<template>
  <div class="past-trips">
    <h1>Tidligere ture</h1>
    <div class="timeline" v-if="pastTrips.length > 0">
      <TripTimelineItem v-for="trip in pastTrips" :key="trip.year" :trip="trip" />
    </div>
    <p v-else class="empty">Ingen tidligere ture at vise.</p>
  </div>
</template>


<script lang="ts">
import { defineComponent } from 'vue';
import { $api } from '../common/apiService';
import { Trip } from '@/Models/Trip';
import TripTimelineItem from '@/components/TripTimelineItem.vue';

export default defineComponent({
  name: 'PastTripsPage',
  components: { TripTimelineItem },
  data() {
    return {
      pastTrips: [] as Trip[],
    };
  },
  async mounted() {
    const trips = await $api.trips.fetch();
    if (trips) {
      const today = new Date();
      this.pastTrips = trips.filter(t => new Date(t.startDate) < today);
    }
  },
});
</script>


<style scoped>
@import '../colors.css';

.past-trips {
  padding: 36px 30px;
}

h1 {
  font-size: 32px;
  font-weight: 300;
  margin-bottom: 48px;
  color: var(--col4);
}

.timeline {
  position: relative;
  display: flex;
  flex-direction: column;
  gap: 24px;
}

.timeline::before {
  content: '';
  position: absolute;
  top: 0;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  width: 1px;
  background-color: var(--col3);
}

.empty {
  color: #888;
  font-size: 18px;
}

@media only screen and (max-width: 700px) {
  .past-trips {
    padding: 28px 16px 28px 32px;
  }

  .timeline::before {
    left: -16px;
    transform: none;
  }
}

@media only screen and (min-width: 1000px) {
  .past-trips {
    padding: 50px 60px;
  }
}
</style>
