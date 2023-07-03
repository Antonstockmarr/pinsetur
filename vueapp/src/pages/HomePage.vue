<template>
    <div class="grid">
        <div class="banner-image tile">
            <img :src="cover ?? require('@/assets/Loading_icon.gif')">
        </div>
        <div class="description tile">
            <p>{{description}}</p>
        </div>
        <div class="next-trip tile">
            <h1>Næste tur</h1>
            <TripTile :trip="nextTrip" v-if="nextTrip"></TripTile>
        </div>
        <div class="trip-slider tile">
            <h1>Tidligere ture</h1>
            <div class="previous-trip-cards" v-if="previousTrips">
                <div v-for="trip in previousTrips" :key="trip.year" class="previous-trip-card">
                    <TripTile :trip="trip" :card-style="'min-width: calc(25vw);'"></TripTile>
                </div>
            </div>
        </div>
    </div>
</template>


<script lang="ts">
import { defineComponent } from 'vue';
import json from '../assets/Text.json'
import { $api } from '../common/apiService'
import { Image } from "@/Models/Image";
import TripTile from '@/components/TripTile.vue'
import { Trip } from '@/Models/Trip';

export default defineComponent ({
    name: "HomePage",
    components: {
        TripTile
    },
    data () {
        return {
            description: json.description,
            cover: undefined as string | undefined,
            test: null as Image[] | null,
            nextTrip: null as Trip | null,
            previousTrips: null as Trip[] | null
        }
    },
    async mounted() {
        const trips = await $api.trips.fetch();
        if (trips && trips.length > 0) {
            this.nextTrip = trips.shift() ?? null;
            this.previousTrips = trips;
        }
        if (this.previousTrips) {
            for (let i = 0; i < this.previousTrips.length; i++) {
                const coverId = this.previousTrips[i].coverImageId;
                if (coverId) {
                    this.cover = await $api.images.download(coverId) ?? undefined;
                    break;
                }
            }
        }
    }
});

</script>

<style scoped>

.grid {
    display: grid;
    grid-template-columns: 1fr 1fr 1fr;
    gap: 30px;
    padding: 30px 30px;
}

.banner-image {
    grid-column: 1 / span 2;
    height: fit-content;
    
    & img {
        width: 100%;
        height: 100%;
    }
}

.description {
    padding: 3em;
    line-height: 2em;
    font-size: 1em;
    align-content: center;
}

.trip-slider  {
    grid-column: 2 / span 2;
}

.previous-trip-cards {
    display: flex;
    overflow-y: auto;
}

.previous-trip-card {
    border: 1px solid black;
    margin: 10px;
}

.tile {
    border: 2px black solid;
    box-shadow: 6px 6px 2px 1px rgba(0, 0, 0, .5);
}

h1 {
    font-size: 24px;
    text-align: center;
}

</style>