<template>
  <div class="timeline-item">
    <router-link :to="`/trips/${trip.year}`" class="timeline-card">
      <div class="timeline-image" :style="image ? { backgroundImage: `url(${image})` } : {}" />
      <div class="timeline-content">
        <span class="timeline-eyebrow">{{ trip.year }}</span>
        <h2 class="timeline-title">{{ trip.location }}</h2>
        <p class="timeline-dates">{{ formatDateRange(trip.startDate, trip.endDate) }}</p>
        <span class="timeline-link">Se turen &rarr;</span>
      </div>
    </router-link>
    <div class="timeline-dot" />
  </div>
</template>


<script lang="ts">
import { Trip } from '@/Models/Trip';
import { defineComponent } from 'vue';
import type { PropType } from 'vue';
import { $api } from '@/common/apiService';

export default defineComponent({
  name: 'TripTimelineItem',
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

.timeline-item {
  position: relative;
  width: 50%;
}

.timeline-item:nth-child(odd) {
  padding-right: 40px;
  align-self: flex-start;
}

.timeline-item:nth-child(even) {
  padding-left: 40px;
  align-self: flex-start;
  margin-left: 50%;
}

.timeline-card {
  display: flex;
  flex-direction: column;
  text-decoration: none;
  border-radius: 4px;
  overflow: hidden;
  background-color: white;
  box-shadow: 0 2px 16px rgba(0, 0, 0, 0.08);
  transition: box-shadow 0.3s ease, transform 0.3s ease;
}

.timeline-card:hover {
  box-shadow: 0 6px 28px rgba(0, 0, 0, 0.14);
  transform: translateY(-2px);
}

.timeline-image {
  aspect-ratio: 16 / 9;
  background-color: var(--col5);
  background-size: cover;
  background-position: center;
}

.timeline-content {
  padding: 22px 24px 24px;
}

.timeline-eyebrow {
  display: block;
  font-size: 11px;
  font-weight: 600;
  letter-spacing: 4px;
  text-transform: uppercase;
  color: var(--col5);
  margin-bottom: 6px;
}

.timeline-title {
  font-size: 22px;
  font-weight: 300;
  color: var(--col4);
  margin: 0 0 6px;
  line-height: 1.2;
}

.timeline-dates {
  font-size: 13px;
  color: #888;
  margin: 0 0 14px;
  letter-spacing: 0.2px;
}

.timeline-link {
  font-size: 12px;
  font-weight: 500;
  letter-spacing: 2px;
  text-transform: uppercase;
  color: var(--col4);
  text-decoration: underline;
  text-underline-offset: 4px;
  text-decoration-color: rgba(2, 18, 48, 0.3);
}

.timeline-dot {
  position: absolute;
  top: 20px;
  width: 16px;
  height: 16px;
  border-radius: 50%;
  background-color: var(--col4);
  border: 3px solid var(--col2);
  z-index: 1;
}

.timeline-item:nth-child(odd) .timeline-dot {
  right: -8px;
}

.timeline-item:nth-child(even) .timeline-dot {
  left: -8px;
}

/* ── Mobile ──────────────────────────────────────────────── */
@media only screen and (max-width: 700px) {
  .timeline-item,
  .timeline-item:nth-child(even) {
    width: 100%;
    margin-left: 0;
    padding-left: 24px;
    padding-right: 0;
  }

  .timeline-item:nth-child(odd) .timeline-dot,
  .timeline-item:nth-child(even) .timeline-dot {
    left: -8px;
    right: auto;
  }
}
</style>
