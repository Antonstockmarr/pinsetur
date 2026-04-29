<template>
  <div class="future-trips">
    <h1>Kommende ture</h1>
    <BCardGroup deck v-if="futureTrips.length > 0">
      <TripTile v-for="trip in futureTrips" :key="trip.year" :trip="trip" />
    </BCardGroup>
    <p v-else class="empty">Ingen kommende ture er planlagt endnu.</p>
  </div>
</template>


<script lang="ts">
import { defineComponent } from 'vue';
import { $api } from '../common/apiService';
import { Trip } from '@/Models/Trip';
import TripTile from '@/components/TripTile.vue';

export default defineComponent({
  name: 'FutureTripsPage',
  components: { TripTile },
  data() {
    return {
      futureTrips: [] as Trip[],
    };
  },
  async mounted() {
    const trips = await $api.trips.fetch();
    if (trips) {
      const today = new Date();
      this.futureTrips = trips.filter(t => new Date(t.startDate) >= today);
    }
  },
});
</script>


<style scoped>
.future-trips {
  padding: 36px 30px;
}

h1 {
  font-size: 32px;
  font-weight: 300;
  margin-bottom: 30px;
}

.empty {
  color: #888;
  font-size: 18px;
}

@media only screen and (min-width: 1000px) {
  .future-trips {
    padding: 50px 60px;
  }
}
</style>
