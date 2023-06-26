<template>
    <div class="post">
        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="post" class="content">
            <table>
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="forecast in post" :key="forecast.date">
                        <td>{{ forecast.date }}</td>
                        <td>{{ forecast.temperatureC }}</td>
                        <td>{{ forecast.temperatureF }}</td>
                        <td>{{ forecast.summary }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div v-if="blobs" class="content">
            <table>
                <thead>
                    <tr>
                        <th>Uri</th>
                        <th>Name</th>
                        <th>ContentType</th>
                        <th>Content</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="blob in blobs" :key="blob.name">
                        <td>{{ blob.uri }}</td>
                        <td>{{ blob.name }}</td>
                        <td>{{ blob.contentType }}</td>
                        <td>{{ blob.content }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script lang="ts">
    import { BlobDto } from '@/Models/BlobDto';
    import { defineComponent } from 'vue';

    type Forecasts = {
        date: string,
        temperatureC: string,
        temperatureF: string,
        summary: string
    }[];

    interface Data {
        loading: boolean,
        post: null | Forecasts
        blobs: null | BlobDto[]
    }

    export default defineComponent({
        data(): Data {
            return {
                loading: false,
                post: null,
                blobs: null
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData(): void {
                this.blobs = null
                this.post = null;
                this.loading = true;

                fetch('weatherforecast')
                    .then(r => r.json())
                    .then(json => {
                        this.post = json as Forecasts;
                        this.loading = false;
                        return;
                    });

                fetch('storage')
                    .then(r => r.json())
                    .then(json => {
                        this.blobs = json as BlobDto[];
                        this.loading = false;
                        return;
                    });
            }
        },
    });
</script>