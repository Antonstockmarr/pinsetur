
<template>
    <b-modal
        v-model:model-value="show"
        :title="title"
        :ok-title="confirmText ?? 'Bekræft'"
        :cancel-title="cancelText ?? 'Fortryd'"
        @cancel="cancel"
        @ok="confirm"
    >
        <p>{{ description }}</p>
    </b-modal>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    name: 'ConfirmModal',
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
        description: {
            type: String,
            required: true
        },
        confirmText: {
            type: String,
            required: false
        },
        cancelText: {
            type: String,
            required: false
        }
    },
    emits: ['update:showModal', 'confirmed'],
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
    methods: {
        confirm() {
            this.show = false
            this.$emit('confirmed')
        },
        cancel() {
            this.show = false
        }
    }

});
</script>

<style scoped>
</style>
    