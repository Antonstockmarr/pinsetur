<template>
  <div class="past-trips">
    <h1>Tidligere ture</h1>
    <div class="trip-grid" v-if="pastTrips.length > 0">
      <TripTile v-for="trip in pastTrips" :key="trip.year" :trip="trip" />
    </div>
    <p v-else class="empty">Ingen tidligere ture at vise.</p>
  </div>
</template>


<script lang="ts">
import { defineComponent } from 'vue';
import { $api } from '../common/apiService';
import { Trip } from '@/Models/Trip';
import TripTile from '@/components/TripTile.vue';

export default defineComponent({
  name: 'PastTripsPage',
  components: { TripTile },
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
  margin-bottom: 30px;
  color: var(--col4);
}

.trip-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 16px;
}

.empty {
  color: #888;
  font-size: 18px;
}

@media only screen and (max-width: 1000px) {
  .trip-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media only screen and (max-width: 600px) {
  .past-trips {
    padding: 28px 16px;
  }

  .trip-grid {
    grid-template-columns: 1fr;
  }
}

@media only screen and (min-width: 1000px) {
  .past-trips {
    padding: 50px 60px;
  }
}
</style>
