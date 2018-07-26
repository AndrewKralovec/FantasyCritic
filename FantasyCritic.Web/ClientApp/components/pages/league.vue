<template>
    <div v-if="league">
        <div v-if="errorInfo" class="alert alert-danger" role="alert">
            {{errorInfo}}
        </div>
        <div v-if="invitedEmail" class="alert alert-success" role="alert">
            Sucessfully sent invite to {{ invitedEmail }}!
        </div>
        <h2>{{ league.leagueName }}</h2>
        <div class="col-md-8" v-if="league.outstandingInvite">
            You have been invited to join this league. Do you wish to join?
            <div class="row">
                <div class="btn-toolbar">
                    <b-button variant="primary" v-on:click="acceptInvite" class="mx-2">Join</b-button>
                    <b-button variant="secondary" v-on:click="declineInvite" class="mx-2">Decline</b-button>
                </div>
            </div>

        </div>
        <div>
            <h3>League Manager</h3>
            {{ league.leagueManager.userName }}
        </div>
        <h3>Players</h3>
        <ul>
            <li v-for="player in league.players">
                {{ player.userName }}
            </li>
        </ul>
        <h3>Invited Players</h3>
        <ul>
            <li v-for="player in league.invitedPlayers">
                {{ player.userName }}
            </li>
        </ul>
        <div class="col-md-8" v-if="league.isManager">
            <div v-if="!showInvitePlayer">
                <b-button variant="info" class="nav-link" v-on:click="showInvite">Invite a Player</b-button>
            </div>
            <div v-if="showInvitePlayer">
                <form method="post" class="form-horizontal" role="form" v-on:submit.prevent="invitePlayer">
                    <div class="form-group col-md-4">
                        <label for="inviteEmail" class="control-label">Email Address</label>
                        <input v-model="inviteEmail" id="inviteEmail" name="inviteEmail" type="text" class="form-control input" />
                    </div>
                    <div class="form-group col-md-2">
                        <input type="submit" class="btn btn-primary" value="Send Invite" />
                    </div>
                </form>
                <div v-if="inviteSent">
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import Vue from "vue";
    import axios from "axios";
    export default {
        data() {
            return {
                errorInfo: "",
                leagueID: "",
                league: null,
                showInvitePlayer: false,
                inviteEmail: "",
                invitedEmail: ""
            }
        },
        methods: {
            fetchLeague() {
                this.leagueID = this.$route.params.id;
                axios
                    .get('/api/League/GetLeague/' + this.leagueID)
                    .then(response => {
                        this.league = response.data;
                    })
                    .catch(returnedError => (this.error = returnedError));
            },
            showInvite() {
                this.showInvitePlayer = true;
            },
            acceptInvite() {
                var model = {
                    leagueID: this.leagueID
                };
                axios
                    .post('/api/league/AcceptInvite', model)
                    .then(response => {
                        this.fetchLeague();
                    })
                    .catch(response => {

                    });
            },
            declineInvite() {
                var model = {
                    leagueID: this.leagueID
                };
                axios
                    .post('/api/league/DeclineInvite', model)
                    .then(response => {
                        this.$router.push({ name: "home" });
                    })
                    .catch(response => {

                    });
            },
            invitePlayer() {
                var model = {
                    leagueID: this.leagueID,
                    inviteEmail: this.inviteEmail
                };
                axios
                    .post('/api/league/InvitePlayer', model)
                    .then(response => {
                        this.showInvitePlayer = false;
                        this.invitedEmail = this.inviteEmail;
                        this.inviteEmail = "";
                        this.fetchLeague();
                    })
                    .catch(response => {
                        this.errorInfo = "Cannot find a player with that email address."
                    });
            }
        },
        mounted() {
            this.fetchLeague();
        },
        watch: {
            '$route'(to, from) {
                this.fetchLeague();
            }
        }
    }
</script>