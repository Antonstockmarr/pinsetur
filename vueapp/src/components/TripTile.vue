<template>
  <router-link :to="`/trips/${trip.year}`" class="trip-tile">
    <div class="tile-image" :style="image ? { backgroundImage: `url(${image})` } : {}">
      <div class="tile-gradient" />
      <div class="tile-content">
        <span class="tile-eyebrow">{{ trip.year }}</span>
        <h2 class="tile-title">{{ trip.location }}</h2>
        <p class="tile-dates">{{ formatDateRange(trip.startDate, trip.endDate) }}</p>
      </div>
    </div>
  </router-link>
</template>


<script lang="ts">
import { Trip } from '@/Models/Trip';
import { defineComponent } from 'vue';
import type { PropType } from 'vue';
import { $api } from '@/common/apiService';

export default defineComponent({
  name: 'TripTile',
  props: {
    trip: {
      type: Object as PropType<Trip>,
      required: true,
    },
  },
  data() {
    return {
      image: null as string | null,
    };
  },
  async mounted() {
    const token = this.$store.getters.getSasToken;
    if (this.trip.coverImageId != null) {
      const img = await $api.images.get(this.trip.coverImageId);
      this.image = img ? img.uri + '?' + token : null;
    } else if (this.trip.locationImageId != null) {
      const img = await $api.images.get(this.trip.locationImageId);
      this.image = img ? img.uri + '?' + token : null;
    }
  },
  methods: {
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

.trip-tile {
  display: block;
  text-decoration: none;
  border-radius: 4px;
  overflow: hidden;
}

.tile-image {
  position: relative;
  aspect-ratio: 3 / 2;
  background-color: var(--col5);
  background-size: cover;
  background-position: center;
  transition: transform 0.45s ease;
}

.trip-tile:hover .tile-image {
  transform: scale(1.03);
}

.tile-gradient {
  position: absolute;
  inset: 0;
  background: linear-gradient(
    to top,
    rgba(2, 18, 48, 0.85) 0%,
    rgba(2, 18, 48, 0.1) 55%,
    transparent 100%
  );
}

.tile-content {
  position: absolute;
  bottom: 24px;
  left: 24px;
  right: 24px;
}

.tile-eyebrow {
  display: block;
  font-size: 11px;
  font-weight: 600;
  letter-spacing: 4px;
  text-transform: uppercase;
  color: var(--col1);
  margin-bottom: 6px;
}

.tile-title {
  font-size: 26px;
  font-weight: 300;
  color: white;
  margin: 0 0 4px;
  line-height: 1.15;
}

.tile-dates {
  font-size: 13px;
  font-weight: 300;
  color: var(--col2);
  margin: 0;
  letter-spacing: 0.3px;
}
</style>
