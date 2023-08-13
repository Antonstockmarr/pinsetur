<template>
    <b-modal
        size="xl"
        hide-footer
        v-model:model-value="showCarousel"
    >
        <div class="carousel-container">
            <b-carousel controls
                v-model="slide"
            >
                <b-carousel-slide
                    v-for="image in images" :img-src="image.uri + '?' + token"
                    v-bind:key="image.id"
                >
                    <template v-slot:img>
                        <img
                        class="d-block carousel-image"
                        :src="image.uri + '?' + token"
                        alt="image">
                    </template>
                </b-carousel-slide>
            </b-carousel>            
        </div>
    </b-modal>

</template>

<script lang="ts">
import { PropType, defineComponent } from 'vue';
import { Image } from '@/Models/Image';

export default defineComponent ({
    name: "ImageCarousel",
    components: {
    },
    props: {
        images: {
            type: Object as PropType<Image[]>,
            required: true
        },
        selectedIndex: {
            type: Number,
            required: true
        },
        show: {
            type: Boolean,
            required: true
        },
        token: {
            type: String,
            required: true
        }
    },
    emits: ['update:show', 'update:selectedImage'],
    computed: {
        showCarousel: {
            get() {
                return this.show
            },
            set(value: boolean) {
                this.$emit('update:show', value)
            }
        },
        slide: {
            get() {
                return this.selectedIndex
            },
            set(value: number) {
                this.$emit('update:selectedImage', value)
            }
        }
    },
    data() {
        return {
            
        }
    },
    methods: {
    }
});
</script>

<style scoped>
.carousel-container {
    height: 85vh;
    overflow: hidden;
}

.carousel-image {
    width: 100%;
}
</style>