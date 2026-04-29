<template>
  <div class="home">

    <!-- HERO — always most recent past trip -->
    <HeroSection
      v-if="pastTrip"
      :images="heroImages"
      eyebrow="Seneste tur"
      :title="pastTrip.location"
      :dates="formatDateRange(pastTrip.startDate, pastTrip.endDate)"
      :link-to="`/trips/${pastTrip.year}`"
    />

    <!-- UPCOMING TRIP STRIP -->
    <div class="upcoming" v-if="futureTrip">
      <div class="upcoming-inner">
        <span class="upcoming-eyebrow">Næste tur</span>
        <h2 class="upcoming-title">{{ futureTrip.location }}</h2>
        <p class="upcoming-dates">{{ formatDateRange(futureTrip.startDate, futureTrip.endDate) }}</p>
        <router-link :to="`/trips/${futureTrip.year}`" class="upcoming-link">
          Se turen &rarr;
        </router-link>
      </div>
    </div>

    <!-- WELCOME TEXT -->
    <div class="welcome" v-if="config?.welcomeTitle || config?.welcomeText">
      <h2 v-if="config?.welcomeTitle">{{ config.welcomeTitle }}</h2>
      <p v-if="config?.welcomeText">{{ config.welcomeText }}</p>
    </div>

    <!-- COLLAGE STRIP -->
    <div class="collage-section" v-if="collageImages.length > 0 && pastTrip">
      <router-link :to="`/trips/${pastTrip.year}`" class="collage-link">
        <div class="collage-strip">
          <img v-for="(img, idx) in collageImages" :key="idx" :src="img" />
        </div>
      </router-link>
      <p class="upload-prompt">
        {{ config?.uploadPrompt }}
        <router-link :to="`/trips/${pastTrip.year}?upload=true`">Upload dem her.</router-link>
      </p>
    </div>

  </div>
</template>


<script lang="ts">
import { defineComponent } from 'vue';
import { $api } from '../common/apiService';
import { Trip } from '@/Models/Trip';
import { HomepageConfig } from '@/Models/HomepageConfig';
import HeroSection from '@/components/HeroSection.vue';

export default defineComponent({
  name: 'HomePage',
  components: { HeroSection },
  data() {
    return {
      config: null as HomepageConfig | null,
      pastTrip: null as Trip | null,
      futureTrip: null as Trip | null,
      heroImages: [] as string[],
      collageImages: [] as string[],
    };
  },
  async mounted() {
    this.config = await $api.homepageConfig.get();

    const token = this.$store.getters.getSasToken;
    const trips = await $api.trips.fetch();
    if (!trips || trips.length === 0) return;

    const today = new Date();
    const past = trips.filter(t => new Date(t.startDate) < today);
    const future = trips.filter(t => new Date(t.startDate) >= today);

    this.pastTrip = past[0] ?? null;
    this.futureTrip = future[0] ?? null;

    await Promise.all([
      this.loadHeroImages(token),
      this.loadCollageImages(token),
    ]);
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
        if (!this.pastTrip) return;
        const pt = this.pastTrip;

        // Hero uses only gallery images — cover + location are reserved for the collage
        const gallery = await $api.images.fetch(pt.year);
        if (gallery) {
          this.heroImages = gallery
            .filter(img => img.id !== pt.coverImageId && img.id !== pt.locationImageId)
            .slice(0, 5)
            .map(img => img.uri + '?' + token);
        }
      }

    },

    async loadCollageImages(token: string) {
      if (!this.pastTrip) return;
      const pt = this.pastTrip;
      const images: string[] = [];

      // Collage uses the trip's curated images (cover + location) — always different from hero
      if (pt.coverImageId) {
        const img = await $api.images.get(pt.coverImageId);
        if (img) images.push((img.thumbUri || img.uri) + '?' + token);
      }
      if (pt.locationImageId && images.length < 3) {
        const img = await $api.images.get(pt.locationImageId);
        if (img) images.push((img.thumbUri || img.uri) + '?' + token);
      }

      // Fill remaining slots from gallery if needed
      if (images.length < 3) {
        const curatedIds = new Set(this.config?.frontpageImageIds ?? []);
        const gallery = await $api.images.fetch(pt.year);
        if (gallery) {
          const remaining = gallery
            .filter(img => img.id !== pt.coverImageId && img.id !== pt.locationImageId)
            .filter(img => !curatedIds.has(img.id));
          images.push(
            ...remaining
              .slice(0, 3 - images.length)
              .map(img => (img.thumbUri || img.uri) + '?' + token)
          );
        }
      }

      this.collageImages = images;
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

/* ── Upcoming trip strip ─────────────────────────────────── */
.upcoming {
  background-color: var(--col4);
  border-top: 1px solid rgba(255, 255, 255, 0.08);
}

.upcoming-inner {
  padding: 44px 64px;
}

.upcoming-eyebrow {
  display: block;
  font-size: 11px;
  font-weight: 600;
  letter-spacing: 4px;
  text-transform: uppercase;
  color: var(--col1);
  margin-bottom: 10px;
}

.upcoming-title {
  font-size: 38px;
  font-weight: 300;
  color: white;
  margin: 0 0 8px;
  line-height: 1.1;
}

.upcoming-dates {
  font-size: 16px;
  font-weight: 300;
  color: var(--col2);
  margin: 0 0 22px;
  letter-spacing: 0.3px;
}

.upcoming-link {
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

.upcoming-link:hover {
  color: var(--col1);
  text-decoration-color: var(--col1);
}

/* ── Welcome ─────────────────────────────────────────────── */
.welcome {
  background-color: var(--col2);
  padding: 64px;
  text-align: center;
}

.welcome h2 {
  font-size: 34px;
  font-weight: 300;
  color: var(--col4);
  margin: 0 0 18px;
  line-height: 1.2;
}

.welcome p {
  font-size: 18px;
  line-height: 1.9;
  color: #3a3a3a;
  max-width: 660px;
  margin: 0 auto;
}

/* ── Collage strip ───────────────────────────────────────── */
.collage-section {
  background-color: var(--col2);
  padding-bottom: 52px;
}

.collage-link {
  display: block;
  text-decoration: none;
  overflow: hidden;
}

.collage-strip {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 3px;
}

.collage-strip img {
  width: 100%;
  aspect-ratio: 4 / 3;
  object-fit: cover;
  display: block;
  transition: transform 0.5s ease;
}

.collage-link:hover .collage-strip img {
  transform: scale(1.03);
}

.upload-prompt {
  text-align: center;
  margin-top: 18px;
  font-size: 14px;
  color: #888;
  line-height: 1.7;
  padding: 0 24px;
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

  .upcoming-inner {
    padding: 36px;
  }

  .welcome {
    padding: 48px 36px;
  }

  .upcoming-title {
    font-size: 30px;
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

  .upcoming-inner {
    padding: 28px 20px;
  }

  .upcoming-title {
    font-size: 26px;
  }

  .welcome {
    padding: 36px 20px;
  }

  .welcome h2 {
    font-size: 26px;
  }

  .welcome p {
    font-size: 16px;
  }

  .collage-strip {
    grid-template-columns: repeat(3, 1fr);
    gap: 2px;
  }
}
</style>
