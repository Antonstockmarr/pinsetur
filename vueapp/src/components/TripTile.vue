<template>
    <b-card
        :title="trip?.year.toString()"
        :subtitle="trip?.location"
        :img-src="image"
        :style="injectedCardStyle"
        img-alt="Image"
        img-top
    >
      <template #footer>
        <div class="d-grid gap-2">
          <b-button href="#" variant="primary" :to="`/trips/${trip?.year}`" v-if="trip != null">Se detaljer</b-button>
        </div>
      </template>
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
        image: undefined as string | undefined,
        token: ""
    }
  },
  async mounted() {
    this.token = this.$store.getters.getSasToken
    this.image = require('@/assets/Loading_icon.gif')
    if (this.trip.coverImageId != null) {
      const coverImage = await $api.images.get(this.trip.coverImageId)
      this.image = coverImage?.uri + "?" + this.token;
    }
    else if (this.trip.locationImageId != null) {
      const locationImage = await $api.images.get(this.trip.locationImageId)
      this.image = locationImage?.uri + "?" + this.token;
    }
    else {
      this.image = require("@/assets/NO_IMAGE.jpg");
    }
  },
  computed: {
    injectedCardStyle() {
      if (!this.$store.getters.windowWidthLessThan1000px) {
        return `
          border: 2px black solid;
          box-shadow: 6px 6px 2px 1px rgba(0, 0, 0, .5);
          min-width: 25vw;
        `
        + this.cardStyle;
      }
      else return `
        min-width: 50vw;
        margin-bottom: 10px;
      ` + this.cardStyle
    }
  }
})


</script>
<style scoped>
</style>