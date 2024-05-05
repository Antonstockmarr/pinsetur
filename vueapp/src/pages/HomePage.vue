<template>
    <div class="home-page">
        <div class="grid">
            <ContentCard class="">
                <BCardTitle tag="h1">{{ texts.Homepage.Title }}</BCardTitle>
                <BCardSubtitle tag="h3">{{ texts.Homepage.Subtitle }}</BCardSubtitle>
                <div class="Homepage-items">
                    <BCardText v-for="item, index in texts.Homepage.Items" :key="index">{{item}}</BCardText>
                </div>
            </ContentCard>
            <TripTile v-if="nextTrip" :trip="nextTrip" :style="'min-width: 50vw;'"></TripTile>
            <BImg v-else :src="require('@/assets/Loading_icon.gif')"></BImg>
        </div>
        <h1 class="sub-title">Tidligere Ture</h1>
        <BCardGroup deck v-if="previousTrips">
            <TripTile v-for="trip in previousTrips" :key="trip.year" :trip="trip"></TripTile>
        </BCardGroup>
    </div>
</template>


<script lang="ts">
import { defineComponent } from 'vue';
import texts from '../assets/Text.json'
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
            texts: texts,
            nextTrip: null as Trip | null,
            previousTrips: null as Trip[] | null,
        }
    },
    async mounted() {
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
    padding: 10px;
}

.grid {
    display: grid;
    gap: 15px
}

h1 {
    font-size: 40px;
    margin-bottom: 15px;
}

h3 {
    font-size: 32px;
}

.Homepage-items p {
    font-size: 28px;
    line-height: 40px;
}

@media only screen and (max-width: 600px) {
    h1 {
        font-size: 28px;
    }

    h3 {
        font-size: 24px;
    }

    .Homepage-items p {
        font-size: 18px;
    }
}

@media only screen and (min-width: 1000px) {
    .home-page {
        padding: 30px;
    }

    .grid {
        grid-template-columns: 2fr 1fr;
        gap: 30px;
    }
}

.sub-title {
    font-size: 32px;
    text-align: center;
    margin-top: 30px;
}

</style>