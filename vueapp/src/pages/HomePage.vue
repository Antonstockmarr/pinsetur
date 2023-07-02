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
            <TripTile :trip="nextTrip"></TripTile>
        </div>
        <div class="trip-slider tile">
            <h1>Tidligere ture</h1>
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
            nextTrip: null as Trip | null
        }
    },
    async mounted() {
        const covers = await $api.images.fetch(null, true);
        if (covers != null && covers.length > 0) {
            const latestCover = covers[0];
            this.cover = await $api.images.download(latestCover.id);
        }

        this.nextTrip = await $api.trips.get(2022);
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

.tile {
    border: 2px black solid;
    box-shadow: 6px 6px 2px 1px rgba(0, 0, 0, .5);
}

h1 {
    font-size: 24px;
    text-align: center;
}

</style>