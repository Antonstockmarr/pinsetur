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
                    <div class="tab-content">
                        <b-table striped hover :items="users" :fields="userFields">
                            <template #cell(actions)="row">
                                <b-button size="sm" @click="resetPassword(row)" class="mr-2">
                                    Nulstil kodeord
                                </b-button>
                            </template>
                        </b-table>
                    </div>
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
import { TableItem } from 'bootstrap-vue-next';
import { TableFieldObject } from 'bootstrap-vue-next/dist/src/types';

type RowType = {
    value: unknown;
    index: number;
    item: TableItem;
    field: TableFieldObject<Record<string, unknown>>;
    items: TableItem[];
    toggleDetails: () => void;
    detailsShowing: boolean | undefined;
}

export default defineComponent ({
    name: "AdminPage",
    components: {
    },
    data() {
        return {
            users: [] as User[],
            userFields: [
                {
                    key: 'userName',
                    label: 'Brugernavn'
                },
                {
                    key: 'name',
                    label: 'Navn'
                },
                {
                    key: 'role',
                    label: 'Rolle'
                },
                {
                    key: 'resetPassword',
                    label: 'Anmod om nyt kodeord'
                },
                {
                    key: 'actions',
                    label: 'Handlinger'
                }
            ]
        }
    },
    methods: {
        async resetPassword(row: RowType) {
            let user = row.item as unknown as User
            const newPassword = await $api.users.resetPassword(user.userName)
            console.log(newPassword)
            await this.fetchUsers()
        },
        async fetchUsers() {
            const users = await $api.users.fetch()
            if (users) {
                this.users = users
            }
        }
    },
    async mounted() {
        await this.fetchUsers();
    }
});

</script>

<style>
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