<template>
  <div class="hero">
    <div
      v-for="(img, idx) in images"
      :key="idx"
      class="hero-slide"
      :class="{ active: idx === currentSlide }"
      :style="{ backgroundImage: `url(${img})` }"
    />
    <div class="hero-gradient" />
    <div class="hero-content" v-if="title">
      <span class="hero-eyebrow" v-if="eyebrow">{{ eyebrow }}</span>
      <h1 class="hero-title">{{ title }}</h1>
      <p class="hero-dates" v-if="dates">{{ dates }}</p>
      <router-link v-if="linkTo" :to="linkTo" class="hero-link">
        Se turen &rarr;
      </router-link>
    </div>
  </div>
</template>


<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'HeroSection',
  props: {
    images: {
      type: Array as () => string[],
      default: () => [],
    },
    eyebrow: {
      type: String,
      default: '',
    },
    title: {
      type: String,
      required: true,
    },
    dates: {
      type: String,
      default: '',
    },
    linkTo: {
      type: String,
      default: '',
    },
  },
  data() {
    return {
      currentSlide: 0,
      slideInterval: null as ReturnType<typeof setInterval> | null,
    };
  },
  watch: {
    images(newImages: string[]) {
      this.resetSlideshow(newImages);
    },
  },
  mounted() {
    this.resetSlideshow(this.images);
  },
  beforeUnmount() {
    if (this.slideInterval) clearInterval(this.slideInterval);
  },
  methods: {
    resetSlideshow(images: string[]) {
      if (this.slideInterval) clearInterval(this.slideInterval);
      this.currentSlide = 0;
      if (images.length > 1) {
        this.slideInterval = setInterval(() => {
          this.currentSlide = (this.currentSlide + 1) % images.length;
        }, 5000);
      }
    },
  },
});
</script>


<style scoped>
@import '../colors.css';

.hero {
  position: relative;
  height: calc(100vh - 100px);
  min-height: 420px;
  overflow: hidden;
  background-color: var(--col4);
}

.hero-slide {
  position: absolute;
  inset: 0;
  background-size: cover;
  background-position: center;
  opacity: 0;
  transition: opacity 1.2s ease-in-out;
}

.hero-slide.active {
  opacity: 1;
}

.hero-gradient {
  position: absolute;
  inset: 0;
  background: linear-gradient(
    to top,
    rgba(2, 18, 48, 0.88) 0%,
    rgba(2, 18, 48, 0.15) 55%,
    transparent 100%
  );
  z-index: 1;
}

.hero-content {
  position: absolute;
  bottom: 64px;
  left: 64px;
  right: 64px;
  z-index: 2;
}

.hero-eyebrow {
  display: block;
  font-size: 11px;
  font-weight: 600;
  letter-spacing: 4px;
  text-transform: uppercase;
  color: var(--col1);
  margin-bottom: 14px;
}

.hero-title {
  font-size: 72px;
  font-weight: 300;
  line-height: 1.05;
  color: white;
  margin: 0 0 14px;
}

.hero-dates {
  font-size: 18px;
  font-weight: 300;
  letter-spacing: 0.5px;
  color: var(--col2);
  margin: 0 0 32px;
}

.hero-link {
  display: inline-block;
  font-size: 13px;
  font-weight: 500;
  letter-spacing: 2.5px;
  text-transform: uppercase;
  color: white;
  text-decoration: underline;
  text-underline-offset: 5px;
  text-decoration-color: rgba(255, 255, 255, 0.45);
  transition: color 0.2s ease, text-decoration-color 0.2s ease;
}

.hero-link:hover {
  color: var(--col1);
  text-decoration-color: var(--col1);
}

@media only screen and (max-width: 1000px) {
  .hero-title {
    font-size: 48px;
  }
}

@media only screen and (max-width: 600px) {
  .hero-content {
    bottom: 40px;
    left: 28px;
    right: 28px;
  }

  .hero-title {
    font-size: 36px;
  }

  .hero-dates {
    font-size: 15px;
    margin-bottom: 22px;
  }
}
</style>
