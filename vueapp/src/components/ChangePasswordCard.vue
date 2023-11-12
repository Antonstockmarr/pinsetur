<template>
    <div class="card login" :class="{ error: errorMessage }">
        <h1>Sæt nyt kodeord</h1>
        <form class="form-group" @submit="changePassword">
            <input class="form-control" v-model="password1" type="password" placeholder="Kodeord" required>
            <input class="form-control" v-model="password2" type="password" placeholder="Gentag kodeord" required>
            <button class="btn btn-primary" type="submit">Bekræft kodeord</button>
            <p class="error-message" v-if="errorMessage"> {{ errorMessage }}</p>
            <p>Glemt koden? Kontakt en administrater af siden.</p>
        </form>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { $api } from '../common/apiService';
import { store } from '@/store';

export default defineComponent ({
    name: "ChangePasswordCard",
    components: {
    },
    data () {
        return {
            password1: "",
            password2: "",
            errorMessage: "",
            emptyFields: false
        }
    },
    methods: {
        async changePassword() {
            this.errorMessage = ""
            this.emptyFields = false
            if (!this.validatePassword()) {
                return
            }
            let user = await $api.user.changePassword(this.password1)
            store.commit('updateUser', user)
            this.$emit('continue')
        },
        validatePassword() {
            if (this.password1 === "" || this.password2 === "") {
                this.emptyFields = true
                return false
            }
            else return this.password1 === this.password2
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