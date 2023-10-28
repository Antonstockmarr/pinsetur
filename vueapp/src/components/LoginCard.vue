<template>
    <div class="card login" :class="{ error: errorMessage }">
        <h1>Log ind</h1>
        <form class="form-group" @submit="logIn">
            <input class="form-control" v-model="username" type="text" placeholder="Brugernavn" required>
            <input class="form-control" v-model="password" type="password" placeholder="Kodeord" required>
            <button class="btn btn-primary" type="submit">Log ind</button>
            <p class="error-message" v-if="errorMessage"> {{ errorMessage }}</p>
            <p>Glemt koden? Kontakt en administrater af siden.</p>
        </form>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { $api } from '../common/apiService';

export default defineComponent ({
    name: "LoginCard",
    components: {
    },
    data () {
        return {
            username: "",
            password: "",
            errorMessage: "",
            emptyFields: false
        }
    },
    methods: {
        async logIn() {
            this.errorMessage = ""
            this.emptyFields = false
            if (!this.validateLogin()) {
                return
            }
            const session = await $api.auth.login(this.username, this.password)
            if (session === null || session?.jwt === null) {
                this.errorMessage = "Log ind fejlede. Prøv igen."
                return
            }

            this.$store.commit('setSession', session)
            if (session?.user.resetPassword) {
                this.$emit('resetPassword')
            }
            else {
                this.$router.push('/')
            }
        },
        validateLogin() {
            if (this.username === "" || this.password === "") {
                this.emptyFields = true
                return false
            }
            else return true
        }
    }
});

</script>

<style scoped>
p {
    margin-top: 10px;
    line-height: 1rem;
}

.card {
   padding: 20px;
}

.form-group {
    .error-message {
        color: red;
    }

}    

.form-group input {
    margin-bottom: 20px;
}

.error {
   animation-name: errorShake;
   animation-duration: 0.3s;
}

@keyframes errorShake {
   0% {
      transform: translateX(-25px);
   }
   25% {
      transform: translateX(25px);
   }
   50% {
      transform: translateX(-25px);
   }
   75% {
      transform: translateX(25px);
   }
   100% {
      transform: translateX(0);
   }
}

</style>