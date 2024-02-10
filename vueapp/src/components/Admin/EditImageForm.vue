<template>
    <b-offcanvas v-model="show" :title="`Ret billedeoplysninger`">
        <template v-if="loading">
            <b-spinner />
        </template>
        <template v-else>
            <b-img class="image-preview" :src="imageUri" />
            <b-form @submit="updateImage">
                <b-form-group label="Beskrivelse">
                    <b-form-textarea
                        v-model="editableImage.description"
                    >
                    </b-form-textarea>
                </b-form-group>

                <p v-if="error" class="text-danger">{{ error }}</p>
                <b-button class="submit-button" size="lg" type="submit" variant="success">Opdatér</b-button>
            </b-form>
        </template>
    </b-offcanvas>
</template>

<script lang="ts">
import { PropType, defineComponent } from 'vue';
import { Image } from '@/Models/Image';
import { emptyImage } from '@/common/utils';
import { $api } from '@/common/apiService';


export default defineComponent ({
    name: "EditImageForm",
    components: {
    },
    props: {
        showForm: {
            type: Boolean,
            required: true
        },
        image: {
            type: Object as PropType<Image>,
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
        },
        imageUri() {
            return this.editableImage.uri + '?' + this.token
        }
    },
    data() {
        return {
            editableImage: emptyImage() as Image,
            loading: false as Boolean,
            error: "" as String,
            token: ""
        }
    },
    watch: {
        showForm: function() {
            this.error = ""
            this.editableImage = JSON.parse(JSON.stringify(this.image))    
        }
    },
    mounted() {
        this.token = this.$store.getters.getSasToken
        this.editableImage = JSON.parse(JSON.stringify(this.image))
    },
    methods: {
        async updateImage() {
            this.error = ""
            this.loading = true
            const newImage = await $api.images.update(this.editableImage)
            this.loading = false
            if (newImage) {
                this.show = false
                this.$emit('submitted')
            }
            else {
                this.error = "Der opstod en fejl..."
            }
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

.image-preview {
    max-width: 100%;
}

</style>