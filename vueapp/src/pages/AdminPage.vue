<template>
    <div class="back-button">
        <b-button size="lg" @click="$router.back()">
            Tilbage
        </b-button>
    </div>
    <b-tabs>
        <b-tab title="Brugere">
            <div v-for="user in users" v-bind:key="user.userName">
                {{ user.userName }}
            </div>
        </b-tab>
        <b-tab title="Ture"></b-tab>
    </b-tabs>
    Admin control page
</template>


<script lang="ts">
import { User } from '@/Models/User';
import { defineComponent } from 'vue';
import { $api } from '@/common/apiService'


export default defineComponent ({
    name: "AdminPage",
    components: {
    },
    data() {
        return {
            users: [] as User[]
        }
    },
    methods: {
    },
    async mounted() {
        const users = await $api.users.fetch()
        if (users) {
            this.users = users
        }
    }
});

</script>

<style>
.back-button {
    float: left;
    height: 65px;
}
</style>