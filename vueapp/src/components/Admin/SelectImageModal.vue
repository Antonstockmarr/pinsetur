
<template>
    <b-modal
        v-model:model-value="show"
        :title="title"
    >
        <div class="gallery-display-box">
            <div v-for="image in images" :key="image.id" class="image" @click="select(image)">
                <img
                :src="image.uri + '?' + token ?? require('@/assets/Loading_icon.gif')"
                />
            </div>
        </div>

    </b-modal>
</template>

<script lang="ts">
import { PropType, defineComponent } from 'vue'
import { Image } from '@/Models/Image';

export default defineComponent({
    name: 'SelectImageModal',
    components: {
    },
    props: {
        showModal: {
            type: Boolean,
            required: true
        },
        title: {
            type: String,
            required: true
        },
        images: {
            type: Object as PropType<Image[]>,
            required: true
        }
    },
    emits: ['update:showModal', 'selected'],
    computed: {
        show: {
            get() {
                return this.showModal
            },
            set(value: boolean) {
                this.$emit('update:showModal', value)
            }
        }
    },
    data() {
        return {
            token: ""
        }
    },
    mounted() {
        this.token = this.$store.getters.getSasToken
    },
    methods: {
        select(image: Image) {
            this.show = false
            this.$emit("selected", image)
        }
    }

});
</script>

<style scoped>

.gallery-display-box {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    row-gap: 20px;
}

.image {
    cursor: pointer;
    flex-shrink: 1;
    transition-duration: 0.2s;
    border-radius: 10px;
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

</style>
    