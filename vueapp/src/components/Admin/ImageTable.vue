<template>
    <div class="image-table-content">

        <p class="error-text" v-if="error"> {{ error }}</p>

        <b-card no-body>
            <b-tabs card>
                <b-tab v-for="year in years" :title="year.toString()" :key="year">
                    <div class="gallery-display-box">
                        <div v-for="image in imagesByYear(year)" :key="image.id" class="image">
                            <img
                                :src="image.uri + '?' + token ?? require('@/assets/Loading_icon.gif')"
                            />
                            <div class="image-options">
                                <b-button @click="editImage(image)">Ret</b-button>
                                <b-button @click="confirmDeleteImage(image)" variant="danger">Slet</b-button>
                            </div>
                        </div>
                    </div>
                </b-tab>
            </b-tabs>
        </b-card>

        <edit-image-form
            v-model:show-form="showEditImageModal"
            :image="changedImage"
            :users="users"
            v-on:submitted="$emit('refresh')"
        />

        <confirm-modal
            v-model:show-modal="showConfirmDeleteModal"
            :title="'Slet billede'"
            :description="`Er du sikker på at du vil slette dette billede fra ${imageToBeDeleted.year}`"
            v-on:confirmed="deleteImage"
        />

    </div>
</template>


<script lang="ts">
import { PropType, defineComponent } from 'vue';
import EditImageForm from './EditImageForm.vue';
import ConfirmModal from '../ConfirmModal.vue';
import { emptyImage } from '@/common/utils';
import { Image } from '@/Models/Image';
import { $api } from '@/common/apiService'
import { User } from '@/Models/User';

export default defineComponent ({
    name: "ImageTable",
    components: {
        EditImageForm,
        ConfirmModal
},
    props: {
        images: {
            type: Object as PropType<Image[]>,
            required: true
        },
        loadingImages: {
            type: Boolean,
            required: true
        },
        users: {
            type: Object as PropType<User[]>,
            required: true
        }
    },
    computed: {
        loading: {
            get(): boolean {
                return this.loadingImages
            },
            set(value: boolean) {
                this.$emit('update:loadingImages', value)
            }
        },
        years() {
            return this.images.map(image => image.year).filter((value, index, array) => array.indexOf(value) === index)
        }
    },
    data() {
        return {
            error: "",
            token: "",
            ImagesFields: [
                {
                    key: 'year',
                    label: 'År'
                },
                {
                    key: 'actions',
                    label: 'Handlinger'
                }
            ],
            imageToBeDeleted: emptyImage(),
            showConfirmDeleteModal: false,
            changedImage: emptyImage(),
            showEditImageModal: false
        }
    },
    mounted() {
        this.token = this.$store.getters.getSasToken
    },
    methods: {
        confirmDeleteImage(image: Image) {
            this.imageToBeDeleted = image
            this.showConfirmDeleteModal = true
        },
        async deleteImage() {
            this.error = ""
            this.loading = true
            const response = await $api.images.delete(this.imageToBeDeleted)
            if (response === null) {
                this.error = "Der skete en fejl på serveren. Prøv igen."
                this.loading = false
            }
            else {
                this.$emit('refresh')
            }
        },
        imagesByYear(year: number) {
            return this.images.filter(image => image.year === year)
        },
        editImage(image: Image) {
            this.error = ""
            this.changedImage = image
            this.showEditImageModal = true
        }
    },
});
</script>

<style scoped>
.image-table-content {
    padding: 15px;
}

.spinner-center {
    display: flex;
    justify-content: center;
}

.error-text {
    color: red;
}

p {
    line-height: 20px;
}

.gallery-display-box {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    row-gap: 20px;
}

.image {
    flex-shrink: 1;
    transition-duration: 0.2s;
    border-radius: 10px;
    position: relative;
}

.image:hover {
    box-shadow: 6px 6px 2px 1px rgba(0, 0, 0, .5);
}

.image img {
    border-radius: 10px;
    border: 1px solid black;
    width: auto;
    max-height: 200px;
}


.image-options {
  transition: .5s ease;
  opacity: 0;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  -ms-transform: translate(-50%, -50%);
}

.image:hover img {
  opacity: 0.3;
}

.image:hover .image-options {
  opacity: 1;
}

</style>