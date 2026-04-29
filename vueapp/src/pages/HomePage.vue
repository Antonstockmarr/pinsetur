<template>
  <div class="home">

    <!-- HERO -->
    <div class="hero">
      <div
        v-for="(img, idx) in heroImages"
        :key="idx"
        class="hero-slide"
        :class="{ active: idx === currentSlide }"
        :style="{ backgroundImage: `url(${img})` }"
      />
      <div class="hero-gradient" />
      <div class="hero-content" v-if="featuredTrip">
        <span class="hero-eyebrow">{{ isFutureTrip ? 'Næste tur' : 'Seneste tur' }}</span>
        <h1 class="hero-title">{{ featuredTrip.location }}</h1>
        <p class="hero-dates">{{ formatDateRange(featuredTrip.startDate, featuredTrip.endDate) }}</p>
        <router-link :to="`/trips/${featuredTrip.year}`" class="hero-link">
          Se turen &rarr;
        </router-link>
      </div>
    </div>

    <!-- CONTENT -->
    <div class="content" :class="{ 'single-col': collageImages.length === 0 }">

      <div class="welcome">
        <h2>{{ config?.welcomeTitle }}</h2>
        <p>{{ config?.welcomeText }}</p>
      </div>

      <div class="collage" v-if="collageImages.length > 0 && collageTrip">
        <router-link :to="`/trips/${collageTrip.year}`" class="collage-link">
          <div class="collage-grid">
            <img
              v-for="(img, idx) in collageImages"
              :key="idx"
              :src="img"
              :class="{ 'span-rows': idx === 0 }"
            />
          </div>
        </router-link>
        <p class="upload-prompt">
          {{ config?.uploadPrompt }}
          <router-link :to="`/trips/${collageTrip.year}?upload=true`">Upload dem her.</router-link>
        </p>
      </div>

    </div>
  </div>
</template>


<script lang="ts">
import { defineComponent } from 'vue';
import { $api } from '../common/apiService';
import { Trip } from '@/Models/Trip';
import { HomepageConfig } from '@/Models/HomepageConfig';

export default defineComponent({
  name: 'HomePage',
  data() {
    return {
      config: null as HomepageConfig | null,
      featuredTrip: null as Trip | null,
      collageTrip: null as Trip | null,
      heroImages: [] as string[],
      collageImages: [] as string[],
      currentSlide: 0,
      slideInterval: null as ReturnType<typeof setInterval> | null,
    };
  },
  computed: {
    isFutureTrip(): boolean {
      if (!this.featuredTrip) return false;
      return new Date(this.featuredTrip.startDate) >= new Date();
    },
  },
  async mounted() {
    this.config = await $api.homepageConfig.get();

    const token = this.$store.getters.getSasToken;
    const trips = await $api.trips.fetch();
    if (!trips || trips.length === 0) return;

    const today = new Date();
    const past = trips.filter(t => new Date(t.startDate) < today);
    const future = trips.filter(t => new Date(t.startDate) >= today);

    this.featuredTrip = future[0] ?? past[0] ?? null;
    this.collageTrip = past[0] ?? null;

    await Promise.all([
      this.loadHeroImages(token),
      this.loadCollageImages(token),
    ]);
  },
  beforeUnmount() {
    if (this.slideInterval) clearInterval(this.slideInterval);
  },
  methods: {
    async loadHeroImages(token: string) {
      const curatedIds = this.config?.frontpageImageIds ?? [];

      if (curatedIds.length > 0) {
        const fetched = await Promise.all(curatedIds.map(id => $api.images.get(id)));
        this.heroImages = fetched
          .filter(img => img !== null)
          .map(img => img!.uri + '?' + token);
      } else {
        if (!this.featuredTrip) return;
        const ft = this.featuredTrip;
        const images: string[] = [];

        if (ft.coverImageId) {
          const cover = await $api.images.get(ft.coverImageId);
          if (cover) images.push(cover.uri + '?' + token);
        }

        const gallery = await $api.images.fetch(ft.year);
        if (gallery) {
          const filtered = gallery.filter(
            img => img.id !== ft.coverImageId && img.id !== ft.locationImageId
          );
          images.push(
            ...filtered.slice(0, 5 - images.length).map(img => img.uri + '?' + token)
          );
        }

        this.heroImages = images;
      }

      if (this.heroImages.length > 1) {
        this.slideInterval = setInterval(() => {
          this.currentSlide = (this.currentSlide + 1) % this.heroImages.length;
        }, 5000);
      }
    },

    async loadCollageImages(token: string) {
      const curatedIds = this.config?.frontpageImageIds ?? [];

      if (curatedIds.length > 0) {
        const fetched = await Promise.all(curatedIds.slice(0, 3).map(id => $api.images.get(id)));
        this.collageImages = fetched
          .filter(img => img !== null)
          .map(img => (img!.thumbUri || img!.uri) + '?' + token);
      } else {
        if (!this.collageTrip) return;
        const ct = this.collageTrip;
        const gallery = await $api.images.fetch(ct.year);
        if (gallery) {
          const filtered = gallery.filter(
            img => img.id !== ct.coverImageId && img.id !== ct.locationImageId
          );
          this.collageImages = filtered
            .slice(0, 3)
            .map(img => (img.thumbUri || img.uri) + '?' + token);
        }
      }
    },

    formatDateRange(startDate: string, endDate: string): string {
      const start = new Date(startDate);
      const end = new Date(endDate);
      const opts: Intl.DateTimeFormatOptions = { day: 'numeric', month: 'long' };
      const s = start.toLocaleDateString('da-DK', opts);
      const e = end.toLocaleDateString('da-DK', { ...opts, year: 'numeric' });
      return `${s} – ${e}`;
    },
  },
});
</script>


<style scoped>
@import '../colors.css';

/* ── Hero ────────────────────────────────────────────────── */
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

/* ── Content section ─────────────────────────────────────── */
.content {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 80px;
  padding: 80px 64px;
  background-color: var(--col2);
  align-items: start;
}

.content.single-col {
  grid-template-columns: 1fr;
  max-width: 680px;
}

.welcome h2 {
  font-size: 34px;
  font-weight: 300;
  color: var(--col4);
  margin: 0 0 22px;
  line-height: 1.2;
}

.welcome p {
  font-size: 18px;
  line-height: 1.9;
  color: #3a3a3a;
  margin: 0;
}

/* ── Collage ─────────────────────────────────────────────── */
.collage-link {
  display: block;
  text-decoration: none;
  overflow: hidden;
  border-radius: 4px;
}

.collage-grid {
  display: grid;
  grid-template-columns: 3fr 2fr;
  grid-template-rows: 1fr 1fr;
  gap: 4px;
  aspect-ratio: 4 / 3;
  overflow: hidden;
  border-radius: 4px;
}

.collage-grid img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
  transition: transform 0.5s ease;
}

.collage-grid img.span-rows {
  grid-row: 1 / span 2;
}

.collage-link:hover .collage-grid img {
  transform: scale(1.04);
}

.upload-prompt {
  margin-top: 16px;
  font-size: 14px;
  color: #888;
  line-height: 1.7;
}

.upload-prompt a {
  color: var(--col4);
  text-decoration: underline;
  text-underline-offset: 2px;
}

.upload-prompt a:hover {
  color: var(--col5);
}

/* ── Responsive ──────────────────────────────────────────── */
@media only screen and (max-width: 1000px) {
  .hero-title {
    font-size: 48px;
  }

  .content {
    grid-template-columns: 1fr;
    padding: 50px 36px;
    gap: 48px;
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

  .content {
    padding: 40px 20px;
    gap: 36px;
  }

  .welcome h2 {
    font-size: 26px;
  }

  .welcome p {
    font-size: 16px;
  }
}
</style>
