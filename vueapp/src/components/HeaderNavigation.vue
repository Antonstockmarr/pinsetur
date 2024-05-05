<template>
  <div class="header">
    <div class="blank"></div>
    <div class="bar">
      <div class="navigation-button title" @click="goToMainPage">Pinsetur</div>
      <div class="navigation-button item" v-if="adminAccess" @click="goToAdminPage">Admin</div>
      <div class="navigation-button logout" v-if="loggedIn" @click="logOut">Log ud</div>
    </div>
  </div>
</template>
  
<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'HeaderNavigation',
  methods: {
    async logOut() {
      await this.$store.dispatch('logOut')
      location.reload()
    },
    goToMainPage() {
      this.$router.push("/")
    },
    goToAdminPage() {
      this.$router.push("/admin")
    }
  },
  computed: {
    loggedIn() {
      return this.$store.getters.isAuthenticated
    },
    adminAccess() {
      return this.$store.getters.isAdmin
    }
  }
});
</script>
  
<style scoped>
@import "../colors.css";

.blank {
  margin-top: 100px;
}

.bar {
  position: fixed;
  top: 0;
  right: 0;
  left: 0;
  height: 100px;
  background-color: var(--col4);
  z-index: 10;
}

.navigation-button {
  padding: 0 20px;
  font-size: 20px;
  text-transform: uppercase;
  line-height: 100px;
  color: var(--col2);
}

.title {
  float: left;
  font-size: 32px;
}

@media only screen and (max-width: 400px) {
  .title {
    font-size: 20px;
  }

  .item, .logout {
    padding: 0 10px;
    font-size: 14px;
  }
}


.navigation-button:hover {
  cursor: pointer;
  color: var(--col5);
}

.logout {
  float: right;
}

.item {
  float: left;
}
</style>
  