<template>
    <div class="login-page">
        <div class="wallpaper-login"></div>
        <div class="container">
            <div class="row">
                <div class="col-lg-4 col-md-6 col-sm-8 mx-auto">
                    <LoginCard v-if="loginState === 'login'" v-on:loggedIn="afterLogin" />
                    <ChangePasswordCard v-else v-on:continue="goToMain" />
                </div>
            </div>
        </div>
    </div>
</template>


<script lang="ts">
import { defineComponent } from 'vue';
import LoginCard from '@/components/LoginCard.vue'
import ChangePasswordCard from '@/components/ChangePasswordCard.vue'


export default defineComponent ({
    name: "LoginPage",
    components: {
        LoginCard,
        ChangePasswordCard
    },
    data () {
        return {
            loginState: "login" as "login" | "changePassword"
        }
    },
    methods: {
        afterLogin() {
            const user = this.$store.getters.getSession.user
            if (user.resetPassword === true) {
                this.showChangePassword()
            }
            else {
                this.goToMain()
            }
        },
        goToMain() {
            this.$router.push('/')
        },
        showChangePassword() {
            this.loginState = "changePassword"
        }
    }
});

</script>

<style>
.login-page {
   align-items: center;
   display: flex;
   height: calc(100vh - 100px);

   .wallpaper-login {
      background: url('~@/assets/Frontpage.JPG')
         no-repeat center center;
      background-size: cover;
      height: calc(100% - 100px);
      position: absolute;
      width: 100%;
   }
   
   .fade-enter-active,
   .fade-leave-active {
  transition: opacity .5s;
}
   .fade-enter,
   .fade-leave-to {
      opacity: 0;
   }

   h1 {
      margin-bottom: 1.5rem;
   }
}
</style>