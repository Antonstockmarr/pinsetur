<template>
    <b-modal
        v-model:model-value="showCarousel"
        size="xl"
        centered
        hide-header
        hide-footer
    >
        <b-button class="close-button" @click="showCarousel = false">X</b-button>
        <div class="image-container" v-for="image, idx in images" :key="idx">
            <div class="image-showcase" v-if="idx === index">
                <b-button
                    class="navigation-button previous-button"
                    variant="primary"
                    v-if="!first"
                    @click="decrementIndex"
                >
                    {{ `< ` }}
                </b-button>
                <b-button
                    class="navigation-button next-button"
                    variant="primary"
                    v-if="!last"
                    @click="incrementIndex"
                >
                    {{ `>` }}
                </b-button>
                <img :src="image.uri + '?' + token" />
            </div>
            <p v-if="idx === index && images[index]?.description">{{ images[index].description }}</p>
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
        }
    },
    emits: ['update:show', 'update:selectedIndex'],
    computed: {
        showCarousel: {
            get() {
                return this.show
            },
            set(value: boolean) {
                this.$emit('update:show', value)
            }
        },
        index: {
            get() {
                return this.selectedIndex
            },
            set(value: number) {
                this.$emit('update:selectedIndex', value)
            }
        }
    },
    data() {
        return {
            token: "",
            first: false,
            last: false
        }
    },
    watch: {
        show: function() {
            this.first = this.index === 0
            this.last = this.index === this.images.length - 1
        }
    },
    mounted() {
        this.token = this.$store.getters.getSasToken
    },
    methods: {
        incrementIndex() {
            if (this.index < this.images.length - 1) {
                if ( this.index == this.images.length - 2) {
                    this.last = true;
                }
                this.index = this.index + 1
                this.first = false;
            }
        },
        decrementIndex() {
            if (this.index > 0) {
                if (this.index == 1) {
                    this.first = true;
                }

                this.index = this.index - 1
                this.last = false;
            }
        }
    }
});
</script>

<style scoped>
.image-showcase {
    padding: 0 10px;
    width: 100%;
    height: 80vh;
    display: flex;
    justify-content: center;
    align-items: center;    
    & img {
        max-width: 100%;
        max-height: calc(80vh - 16px);
    }
}

.navigation-button {
    position: absolute;
    height: 80px;
    top: calc(50% - 40px)
}

.close-button {
    position: absolute;
    right: 10px;
    top: 10px;
}

.previous-button {
    left: 10px;
}

.next-button {
    right: 10px;
}

</style>