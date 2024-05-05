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
  },
  mounted() {
    let mediaQuery1 = window.matchMedia('(max-width: 999px)');
    this.$store.commit('setWindowWidthLessThan1000px', mediaQuery1.matches)
    window.addEventListener('resize', () => {
      this.$store.commit('setWindowWidthLessThan1000px', mediaQuery1.matches)
    });

    let mediaQuery2 = window.matchMedia('(max-width: 599px)');
    this.$store.commit('setWindowWidthLessThan600px', mediaQuery2.matches)
    window.addEventListener('resize', () => {
      this.$store.commit('setWindowWidthLessThan600px', mediaQuery2.matches)
    });

  }
});
</script>

<style scoped>
@media only screen and (min-width: 1000px) {
  .wrapper {
    max-width: 1500px;
    margin: 0 auto;
  }
}
</style>
