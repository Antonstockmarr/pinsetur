<template>
    <div class="admin-header">
        <div class="back-button">
            <b-button size="lg" @click="$router.back()">
                Tilbage
            </b-button>
        </div>
        <h1>Administrator Kontrolcenter</h1>
    </div>
    <div class="content">
        <b-card no-body>
            <b-tabs card>
                <b-tab title="Brugere">
                    <user-table :users="users" v-model:loading-users="loadingUsers" v-on:refresh="fetchUsers" />
                </b-tab>
                <b-tab title="Ture">
                    <trip-table :trips="trips" v-model:loading-trips="loadingTrips" v-on:refresh="fetchTrips" />
                </b-tab>
                <b-tab title="Billeder">
                    <image-table :images="images" v-model:loading-images="loadingImages" v-on:refresh="fetchImages" />
                </b-tab>
            </b-tabs>
        </b-card>
    </div>
</template>


<script lang="ts">
import { User } from '@/Models/User';
import { defineComponent } from 'vue';
import { $api } from '@/common/apiService'
import UserTable from '@/components/Admin/UserTable.vue';
import TripTable from '@/components/Admin/TripTable.vue';
import { Trip } from '@/Models/Trip';
import ImageTable from '@/components/Admin/ImageTable.vue';
import { Image } from '@/Models/Image';

export default defineComponent ({
    name: "AdminPage",
    components: {
    UserTable,
    TripTable,
    ImageTable
},
    data() {
        return {
            users: [] as User[],
            loadingUsers: false,
            trips: [] as Trip[],
            loadingTrips: false,
            images: [] as Image[],
            loadingImages: false
        }
    },
    methods: {
        async fetchUsers() {
            this.loadingUsers = true
            const users = await $api.users.fetch()
            if (users) {
                this.users = users
            }
            this.loadingUsers = false
        },
        async fetchTrips() {
            this.loadingTrips = true
            const trips = await $api.trips.fetch()
            if (trips) {
                this.trips = trips
            }
            this.loadingTrips = false
        },
        async fetchImages() {
            this.loadingImages = true
            const images = await $api.images.fetch()
            if (images) {
                this.images = images
            }
            this.loadingImages = false
        }
    },
    async mounted() {
        await this.fetchUsers()
        await this.fetchTrips()
        await this.fetchImages()
    }
});

</script>

<style scoped>
.back-button {
    float: left;
    height: 65px;
}

.admin-header {
    display: grid;
    grid-template-columns: 20% 1fr 20%;
    position: relative;
    padding: 30px 30px 0 30px;
    
    & h1 {
        text-align: center;
        margin: 0;
        font-size: 44px;
        line-height: 65px;
    }
}

.content {
    padding: 30px;
}

.tab-content {
    padding: 15px;
}
</style>