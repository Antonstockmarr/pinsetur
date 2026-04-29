<template>
  <div class="past-trips">
    <h1>Tidligere ture</h1>
    <BCardGroup deck v-if="pastTrips.length > 0">
      <TripTile v-for="trip in pastTrips" :key="trip.year" :trip="trip" />
    </BCardGroup>
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
.past-trips {
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
  .past-trips {
    padding: 50px 60px;
  }
}
</style>
