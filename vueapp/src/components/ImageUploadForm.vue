<template>
    <b-offcanvas v-model="show" :title="`Upload billede for ${year}`">
        <template v-if="loading">
            <b-spinner />
        </template>
        <template v-else>
            <b-form @submit="submitImage">
                <b-form-file
                    v-model="imageFile"
                    :accept="['.png', '.jpeg', '.jpg']"
                    required
                >
                </b-form-file>
                <div class="mt-3" v-if="imageFile">
                    <div>Billede:</div> 
                    <b-img class="upload-preview" :src="getPreview()" />
                </div>
                <p v-if="error" class="text-danger">{{ error }}</p>
                <b-button class="submit-upload-button" size="lg" type="submit" variant="success" :disabled="!imageFile">Upload</b-button>
            </b-form>
        </template>
    </b-offcanvas>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { $api } from '@/common/apiService'


export default defineComponent ({
    name: "ImageUploadForm",
    components: {
    },
    props: {
        showForm: {
            type: Boolean,
            required: true
        },
        year: {
            type: Number,
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
            imageFile: null as File | null,
            loading: false as Boolean,
            error: "" as String
        }
    },
    methods: {
        getPreview() {
            if (this.imageFile) {
                return URL.createObjectURL(this.imageFile)
            }
            else {
                return ""
            }
        },
        async submitImage() {
            this.error = ""
            if (this.imageFile) {
                this.loading = true
                const image = await $api.images.upload(this.imageFile, this.year)
                this.loading = false
                if (image) {
                    this.show = false
                    this.$emit('submitted')
                }
                else {
                    this.error = "Der opstod en fejl under upload..."
                }
                this.imageFile = null
            }
            else {
                this.error = "Der er ikke valgt et billede"
            }
        }
    }
});
</script>

<style scoped>
.upload-preview {
    max-width: 100%;
}

.submit-upload-button {
    position: absolute;
    bottom: 30px;
    right: 30px;
    left: 30px;
}
</style>