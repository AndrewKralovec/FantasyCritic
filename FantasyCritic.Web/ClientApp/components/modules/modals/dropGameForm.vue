<template>
  <b-modal id="dropGameForm" ref="dropGameFormRef" size="lg" title="Drop a Game" hide-footer @hidden="clearData">
    <div v-if="errorInfo" class="alert alert-danger" role="alert">
      {{errorInfo}}
    </div>
    <p>
      You can use this form to request to drop a game.
      <br />
      Your league's settings determine how often you can do this.
      <br />
      Drop requests are processed on Sunday Nights. See the FAQ for more info.
    </p>
    <form class="form-horizontal" v-on:submit.prevent="dropGame" hide-footer>
      <div class="form-group">
        <label for="gameToDrop" class="control-label">Game</label>
        <b-form-select v-model="gameToDrop">
          <option v-for="publisherGame in droppableGames" v-bind:value="publisherGame">
            {{ publisherGame.gameName }}
          </option>
        </b-form-select>
      </div>

      <div v-if="gameToDrop">
        <input type="submit" class="btn btn-danger add-game-button" value="Make Drop Game Request" :disabled="isBusy" />
      </div>
      <hr />
      <div v-if="dropResult && !dropResult.success" class="alert bid-error alert-danger">
        <h3 class="alert-heading">Error!</h3>
        <ul>
          <li v-for="error in dropResult.errors">{{error}}</li>
        </ul>
      </div>
    </form>
  </b-modal>
</template>

<script>
  import Vue from "vue";
  import axios from "axios";

  export default {
    data() {
      return {
        dropResult: null,
        gameToDrop: null,
        isBusy: false,
        errorInfo: ""
      }
    },
    computed: {
      formIsValid() {
        return (this.dropMasterGame);
      },
      droppableGames() {
        return _.filter(this.publisher.games, { 'counterPick': false });
      }
    },
    props: ['publisher'],
    methods: {
      dropGame() {
        var request = {
          publisherID: this.publisher.publisherID,
          publisherGameID: this.gameToDrop.publisherGameID
        };
        this.isBusy = true;
        axios
          .post('/api/league/MakeDropRequest', request)
          .then(response => {
            this.isBusy = false;
            this.dropResult = response.data;
            if (!this.dropResult.success) {
              return;
            }
            
            var dropInfo = {
              gameName: this.gameToDrop.gameName,
            };
            this.$emit('dropRequestMade', dropInfo);
            this.$refs.dropGameFormRef.hide();
            this.clearData();
          })
          .catch(response => {
            this.isBusy = false;
            this.errorInfo = response.response.data;
          });
      },
      clearData() {
        this.dropResult = null;
        this.gameToDrop = null;
        
      }
    }
  }
</script>
