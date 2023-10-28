<template>
  <div class="header">
    <div class="blank"></div>
    <div class="bar">
      <div class="title">Pinsetur</div>
      <div class="logout" v-if="loggedIn" @click="logOut">Log ud</div>
    </div>
  </div>
</template>
  
<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'HeaderNavigation',
  data() {
    return {
      baseUrl: window.location.origin
    }
  },
  methods: {
    logOut() {
      location.href = `${this.baseUrl}/.auth/logout`;
    }
  },
  computed: {
    loggedIn() {
      return this.$store.getters.isAuthenticated
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

.title {
  float: left;
  padding: 0 20px;
  font-size: 32px;
  text-transform: uppercase;
  line-height: 100px;
  color: var(--col2);
}

.logout {
  float: right;
  padding: 0 20px;
  font-size: 20px;
  text-transform: uppercase;
  line-height: 100px;
  color: var(--col2);
}

.logout:hover {
  cursor: pointer;
  color: var(--col5);
}

</style>
  