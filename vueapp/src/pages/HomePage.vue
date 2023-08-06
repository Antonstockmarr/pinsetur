<template>
    <div class="home-page">
        <div class="grid">
            <TripTile v-if="nextTrip" :trip="nextTrip" :token="token ?? ''" :style="'min-width: 50vw;'"></TripTile>
            <BImg v-else :src="require('@/assets/Loading_icon.gif')"></BImg>
            <ContentCard>
                <BCardText>{{description}}</BCardText>
            </ContentCard>
        </div>
        <h1 class="sub-title">Tidligere Ture</h1>
        <BCardGroup deck v-if="previousTrips">
            <TripTile v-for="trip in previousTrips" :key="trip.year" :trip="trip" :token="token ?? ''"></TripTile>
        </BCardGroup>
    </div>
</template>


<script lang="ts">
import { defineComponent } from 'vue';
import json from '../assets/Text.json'
import { $api } from '../common/apiService'
import TripTile from '@/components/TripTile.vue'
import ContentCard from '@/components/ContentCard.vue';
import { Trip } from '@/Models/Trip';


export default defineComponent ({
    name: "HomePage",
    components: {
        TripTile,
        ContentCard
    },
    data () {
        return {
            description: json.description,
            nextTrip: null as Trip | null,
            previousTrips: null as Trip[] | null,
            token: null as string | null
        }
    },
    async mounted() {
        this.token = await $api.token.get();
        const trips = await $api.trips.fetch();
        if (trips && trips.length > 0) {
            this.nextTrip = trips.shift() ?? null;
            this.previousTrips = trips;
        }
    }
});

</script>

<style scoped>

.home-page {
    padding: 30px;
}

.grid {
    display: grid;
    grid-template-columns: 2fr 1fr;
    gap: 30px;
}

.sub-title {
    font-size: 32px;
    text-align: center;
    margin-top: 30px;
}

</style>