<template>
  <div class="header">
    <div class="blank"></div>
    <div class="bar">
      <div class="navigation-button title" @click="navigate('/')">Pinsetur</div>

      <!-- Desktop nav -->
      <div class="desktop-nav">
        <div class="navigation-button item" @click="navigate('/trips/past')">Tidligere ture</div>
        <div class="navigation-button item" @click="navigate('/trips/future')">Kommende ture</div>
        <div class="navigation-button item" v-if="adminAccess" @click="navigate('/admin')">Admin</div>
        <div class="navigation-button logout" v-if="loggedIn" @click="logOut">Log ud</div>
      </div>

      <!-- Mobile burger -->
      <button class="burger" :class="{ open: menuOpen }" @click="menuOpen = !menuOpen" aria-label="Menu">
        <span></span>
        <span></span>
        <span></span>
      </button>
    </div>

    <!-- Mobile dropdown -->
    <div class="mobile-menu" v-if="menuOpen" @click="menuOpen = false">
      <div class="mobile-item" @click="navigate('/trips/past')">Tidligere ture</div>
      <div class="mobile-item" @click="navigate('/trips/future')">Kommende ture</div>
      <div class="mobile-item" v-if="adminAccess" @click="navigate('/admin')">Admin</div>
      <div class="mobile-item" v-if="loggedIn" @click="logOut">Log ud</div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { $api } from '../common/apiService'

export default defineComponent({
  name: 'HeaderNavigation',
  data() {
    return {
      menuOpen: false,
    }
  },
  methods: {
    async logOut() {
      this.menuOpen = false
      await $api.auth.logout()
      await this.$store.dispatch('logOut')
      location.reload()
    },
    navigate(path: string) {
      this.menuOpen = false
      this.$router.push(path)
    },
  },
  computed: {
    loggedIn() {
      return this.$store.getters.isAuthenticated
    },
    adminAccess() {
      return this.$store.getters.isAdmin
    },
  },
})
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
  display: flex;
  align-items: center;
}

.navigation-button {
  padding: 0 20px;
  font-size: 16px;
  text-transform: uppercase;
  line-height: 100px;
  color: var(--col2);
  white-space: nowrap;
  cursor: pointer;
}

.navigation-button:hover {
  color: var(--col5);
}

.title {
  font-size: 28px;
  letter-spacing: 1px;
  margin-right: auto;
}

.desktop-nav {
  display: flex;
  align-items: center;
}

.logout {
  padding-right: 24px;
}

/* ── Burger ────────────────────────────────────────────── */
.burger {
  display: none;
  flex-direction: column;
  gap: 5px;
  width: 26px;
  background: transparent;
  border: none;
  cursor: pointer;
  padding: 0;
  margin-right: 24px;
  flex-shrink: 0;
}

.burger span {
  display: block;
  width: 100%;
  height: 2px;
  background: var(--col2);
  border-radius: 2px;
  transition: transform 0.25s ease, opacity 0.2s ease, background 0.2s ease;
  transform-origin: center;
}

.burger:hover span {
  background: var(--col5);
}

.burger.open span:nth-child(1) {
  transform: translateY(7px) rotate(45deg);
}
.burger.open span:nth-child(2) {
  opacity: 0;
  transform: scaleX(0);
}
.burger.open span:nth-child(3) {
  transform: translateY(-7px) rotate(-45deg);
}

/* ── Mobile dropdown ───────────────────────────────────── */
.mobile-menu {
  display: none;
  position: fixed;
  top: 100px;
  left: 0;
  right: 0;
  background-color: var(--col4);
  z-index: 9;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.35);
}

.mobile-item {
  padding: 18px 24px;
  color: var(--col2);
  font-size: 15px;
  text-transform: uppercase;
  letter-spacing: 1.5px;
  cursor: pointer;
  border-bottom: 1px solid rgba(255, 255, 255, 0.06);
  transition: background 0.15s, color 0.15s;
}

.mobile-item:hover {
  background: rgba(255, 255, 255, 0.04);
  color: var(--col5);
}

/* ── Responsive ────────────────────────────────────────── */
@media only screen and (max-width: 700px) {
  .desktop-nav {
    display: none;
  }

  .burger {
    display: flex;
  }

  .mobile-menu {
    display: block;
  }
}
</style>
