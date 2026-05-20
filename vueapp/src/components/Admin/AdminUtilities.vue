<template>
    <b-spinner v-if="loading" />
    <div v-else>
        <b-button @click="generateThumbnails" variant="primary">Generér thumbnails</b-button>
        <p>{{ thumbnailMessage }}</p>
        <b-button @click="setPasswords" variant="primary">Set alle bruger-passwords</b-button>
        <b-form-input v-model="newPassword" />
        <p>{{ passwordMessage }}</p>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { $api } from '@/common/apiService'


export default defineComponent ({
    name: "AdminUtilities",
    data() {
        return {
            loading: false,
            thumbnailMessage: "",
            passwordMessage: "" as String,
            newPassword: ""
        }
    },
    methods: {
        async generateThumbnails() {
            this.loading = true;
            this.thumbnailMessage = await $api.images.generateThumbnails();
            this.loading = false;
        },
        async setPasswords() {
            this.loading = true;
            this.passwordMessage = await $api.users.setPasswords(this.newPassword.toString());
            this.loading = false;
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

.form-content { 
    padding-bottom: 92px;
}

</style>