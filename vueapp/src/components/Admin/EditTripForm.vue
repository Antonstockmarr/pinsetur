<template>
    <b-offcanvas v-model="show" :title="`Opdatér Tur`">
        <template v-if="loading">
            <b-spinner />
        </template>
        <template v-else>
            <b-form @submit="updateTrip">
                <b-form-group label="År">
                    <b-form-input v-model="editableTrip.year" disabled></b-form-input>
                </b-form-group>

                <b-form-group label="Lokation">
                    <b-form-input
                        v-model="editableTrip.location"
                        text="text"
                    >
                    </b-form-input>
                </b-form-group>

                <b-form-group label="Adresse">
                    <b-form-input
                        v-model="editableTrip.address"
                        text="text"
                    >
                    </b-form-input>

                </b-form-group>
                <b-form-group label="Beskrivelse">
                    <b-form-textarea
                        v-model="editableTrip.description"
                    >
                    </b-form-textarea>
                </b-form-group>

                <b-form-group>
                    <b-button @click="changeCoverImage">Skift Coverbillede</b-button>
                    <select-image-modal
                        v-model:show-modal="showChangeCoverImageModal"
                        :title="'Vælg coverbillede'"
                        :images="images"
                        v-on:selected="setCoverImage"
                    />
                    <div class="mt-3" v-if="editableTrip.coverImageId">
                        <b-img class="image-preview" :src="coverImageUri + '?' + token" />
                    </div>
                </b-form-group>

                <b-form-group>
                    <b-button @click="changeLocationImage">Skift billede af stedet</b-button>
                    <select-image-modal
                        v-model:show-modal="showChangeLocationImageModal"
                        :title="'Vælg billede af stedet'"
                        :images="images"
                        v-on:selected="setLocationImage"
                    />
    
                    <div class="mt-3" v-if="editableTrip.locationImageId">
                        <div>Billede af stedet:</div> 
                        <b-img class="image-preview" :src="locationImageUri + '?' + token" />
                    </div>
                </b-form-group>
                
                <p v-if="error" class="text-danger">{{ error }}</p>
                <b-button class="submit-button" size="lg" type="submit" variant="success">Opdatér</b-button>
            </b-form>
        </template>
    </b-offcanvas>
</template>

<script lang="ts">
import { PropType, defineComponent } from 'vue';
import { $api } from '@/common/apiService'
import { emptyTrip } from '@/common/utils';
import { Trip } from '@/Models/Trip';
import { Image } from '@/Models/Image';
import SelectImageModal from './SelectImageModal.vue';

export default defineComponent ({
    name: "EditTripForm",
    components: {
        SelectImageModal
    },
    props: {
        showForm: {
            type: Boolean,
            required: true
        },
        trip: {
            type: Object as PropType<Trip>,
            required: true
        }

    },
    emits: ['update:showForm', 'submitted'],
    computed: {
        show: {
            get() {
                return this.showForm
            },
            set(value: boolean) {
                this.$emit('update:showForm', value)
            }
        }
    },
    data() {
        return {
            editableTrip: emptyTrip() as Trip,
            images: [] as Image[],
            coverImageUri: "",
            locationImageUri: "",
            token: "",
            loading: false as Boolean,
            error: "" as String,
            showChangeCoverImageModal: false,
            showChangeLocationImageModal: false,
        }
    },
    watch: {
        showForm: async function() {
            this.error = ""
            await this.loadEditableTrip()    
            await this.loadImages()
        }
    },
    async mounted() {
        this.token = this.$store.getters.getSasToken
        await this.loadEditableTrip()
    },
    methods: {
        async loadImages() {
            const tripImages = await $api.images.fetch(this.trip.year)
            if (tripImages) {
                this.images = tripImages
            }
        },
        async loadEditableTrip() {
            this.editableTrip = JSON.parse(JSON.stringify(this.trip))
            await this.updatePreviews()
        },
        async updatePreviews() {
            if (this.editableTrip.coverImageId) {
            this.coverImageUri = await this.getImageUri(this.editableTrip.coverImageId)
            }
            if (this.editableTrip.locationImageId) {
                this.locationImageUri = await this.getImageUri(this.editableTrip.locationImageId)
            }
        },
        async updateTrip() {
            this.error = ""
            this.loading = true
            const newTrip = await $api.trips.update(this.editableTrip)
            this.loading = false
            if (newTrip) {
                this.show = false
                this.$emit('submitted')
            }
            else {
                this.error = "Der opstod en fejl..."
            }
        },
        async getImageUri(id: number) {
            const image = await $api.images.get(id)
            if (image) {
                return image.uri
            }
            else return ""
        },
        changeCoverImage() {
            this.showChangeCoverImageModal = true
        },
        changeLocationImage() {
            this.showChangeLocationImageModal = true
        },
        async setCoverImage(image: Image) {
            this.editableTrip.coverImageId = image.id
            await this.updatePreviews()
        },
        async setLocationImage(image: Image) {
            this.editableTrip.locationImageId = image.id
            await this.updatePreviews()
        }
    }
});
</script>

<style scoped>
.submit-button {
    position: absolute;
    bottom: 30px;
    right: 30px;
    left: 30px;
}

p {
    line-height: 20px;
}

.image-preview {
    max-width: 100%;
}

</style>