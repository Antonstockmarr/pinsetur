<template>
    <b-offcanvas v-model="show" :title="`Upload billede for ${year}`">
        <template v-if="loading">
            <b-spinner />
        </template>
        <template v-else>
            <div class="form-content">
                <b-form @submit="submitImage">
                    <b-button onclick="document.getElementById('photo-upload').click()">Vælg billede</b-button>
                    <input type="file"
                        accept=".png,.jpg,.jpeg"
                        id="photo-upload"
                        @change="setUploadedImage"
                        style="display: none;"
                        tabindex="-1"
                    >

                    <div class="mt-3" v-if="imageFile">
                        <b-img class="upload-preview" :src="getPreview()" />
                    </div>

                    <b-form-group
                        v-if="imageFile"
                        label="Beskrivelse (valgfri):"
                        class="image-description"
                    >
                        <b-form-textarea
                            v-model="description"
                            placeholder="Hvad er der på billedet?"
                        />                    
                    </b-form-group>

                    <p v-if="error" class="text-danger">{{ error }}</p>
                    <b-button class="submit-upload-button" size="lg" type="submit" variant="success" :disabled="!imageFile">Upload</b-button>
                </b-form>
            </div>
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
                if (value === false) {
                    this.imageFile = null
                    this.description = ""
                    this.error = ""
                }
            }
        }
    },
    data() {
        return {
            imageFile: null as File | null,
            description: "" as string,
            loading: false as Boolean,
            error: "" as string,
            fileInputKey: 0 as number
        }
    },
    methods: {
        setUploadedImage(payload: Event) {
            let input = payload.target as HTMLInputElement
            this.imageFile = input.files?.item(0) as File
        },
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
                const image = await $api.images.upload(this.imageFile, this.year, this.description)
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
.form-content { 
    padding-bottom: 92px;
}

.upload-preview {
    max-width: 100%;
}

.submit-upload-button {
    position: absolute;
    bottom: 30px;
    right: 30px;
    left: 30px;
}

.image-description {
    margin-top: 20px;
}

</style>