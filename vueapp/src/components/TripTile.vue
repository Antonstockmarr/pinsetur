<template>
    <b-card
        :title="trip?.year.toString()"
        :subtitle="trip?.location"
        :img-src="image ?? require('@/assets/Loading_icon.gif')"
        :style="cardStyle"
        img-alt="Image"
        img-top
    >
        <b-button href="#" variant="primary" :to="`/trips/${trip?.year}`" v-if="trip != null">Se detaljer</b-button>
    </b-card>
</template>


<script lang="ts">
import { Trip } from '@/Models/Trip';
import { defineComponent } from 'vue';
import type { PropType } from 'vue';
import { $api } from '@/common/apiService'

export default defineComponent({
  name: 'TripTile',
  props: {
    trip: {
        type: Object as PropType<Trip>,
        required: true
    },
    cardStyle: {
      type: String,
      required: false
    }
  },
  data() {
    return {
        image: undefined as string | undefined
    }
  },
  methods: {
    seeTrip() {
        console.log("Go To trip " + this.trip?.year)
    }
  },
  async mounted() { 
    if (this.trip.locationImageId != null) {
        this.image = await $api.images.download(this.trip.locationImageId) ?? undefined;
    }
  }
})


</script>


<style scoped>
</style>