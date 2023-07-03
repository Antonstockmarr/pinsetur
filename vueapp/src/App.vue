<template>
  <HeaderNavigation></HeaderNavigation>
  <div class="wrapper">
    <router-view></router-view>
  </div>
</template>

<script lang="ts">
import HeaderNavigation from './components/HeaderNavigation.vue'

export default ({
  name: 'App',
  components: {
    HeaderNavigation
  }
});
</script>

<style>
@import "reset.css";

.wrapper {
  max-width: 1500px;
  margin: auto;
}
</style>
