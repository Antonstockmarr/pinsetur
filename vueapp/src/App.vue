<template>
  <HeaderNavigation></HeaderNavigation>
  <template v-if="usePageWrapper">
    <div class="wrapper">
      <router-view></router-view>
    </div>
  </template>
  <template v-else>
    <router-view></router-view>
  </template>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import HeaderNavigation from './components/HeaderNavigation.vue'

export default defineComponent({
  name: 'App',
  components: {
    HeaderNavigation
  },
  computed: {
    usePageWrapper(): boolean {
      return this.$route.name !== 'login'
    }
  }
});
</script>

<style>
@import "reset.css";

.wrapper {
  max-width: 1500px;
  margin: 0 auto;
}
</style>
