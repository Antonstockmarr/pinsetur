<template>
    <GMapMap
      v-if = "isLoading"
      :center="center"
      :zoom="8"
      map-type-id="terrain"
      style="width: 100%; height: 100%"
  >
    <GMapMarker
        :key="1"
        :position="marker"
        :clickable="true"
        :draggable="true"
    />
  </GMapMap>
</template>


<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'GoogleMap',
  props: {
    address: {
        type: String,
        required: true
    }
  },
  data() {
    return {
      isLoading: false,
      center: {lat: 0, lng: 0},
      marker: 
        {
          lat: 0,
          lng: 0
        }
    }
  },
  mounted() {
    const geocodingUrl = `https://maps.googleapis.com/maps/api/geocode/json?address=${encodeURIComponent(this.address)}&key=${process.env.VUE_APP_GOOGLE_API_KEY}`;
    fetch(geocodingUrl).then(result => result.json())
    .then(result => {
      if (result.status == "OK") {
        const foundAddress = result.results[0];
        const position = {lat: foundAddress.geometry.location.lat, lng: foundAddress.geometry.location.lng };
        this.center = position;
        this.marker = position;
        this.isLoading = true;
      }
      else {
        console.log("There was a problem with the google api:")
        console.log(result)
      }
    });
  }
})

</script>

<style scoped>
.vue-map-container,
.vue-map-container .vue-map {
    width: 100%;
    height: 100%;
}
</style>