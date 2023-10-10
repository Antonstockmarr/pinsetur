<template>
    <h1>login</h1>
    <button @click="logIn">Log in</button>

    user: {{ user?.username }}

    is authenticated: {{ isAuthenticated }}
</template>


<script lang="ts">
import { defineComponent } from 'vue';
import { $api } from '../common/apiService';

export default defineComponent ({
    name: "LoginPage",
    components: {
    },
    data () {
        return {
        }
    },
    computed: {
        user() { return this.$store.state.auth.user},
        isAuthenticated() { return this.$store.getters.isAuthenticated }
    },
    methods: {
        async logIn() {
            const user = await $api.auth.login()
            if (user) {
                this.$store.commit('setUser', user)
            }
            if (user?.jwt) {
                this.$router.push('/')
            }
        }
    }
});

</script>
