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
                <b-tab title="Ture"></b-tab>
            </b-tabs>
        </b-card>
    </div>
</template>


<script lang="ts">
import { User } from '@/Models/User';
import { defineComponent } from 'vue';
import { $api } from '@/common/apiService'
import UserTable from '@/components/Admin/UserTable.vue';

export default defineComponent ({
    name: "AdminPage",
    components: {
    UserTable
},
    data() {
        return {
            users: [] as User[],
            loadingUsers: false
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
        }
    },
    async mounted() {
        await this.fetchUsers();
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