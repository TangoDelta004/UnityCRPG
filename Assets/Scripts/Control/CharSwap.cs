using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Movement;

namespace Control {
    public class CharSwap : MonoBehaviour {

        public Transform playerChar1;
        public Transform playerChar2;
        public Transform playerChar3;
        public Transform playerChar4;

        public Transform currentPlayer;
        int currentPlayerIndex;
        int linkPlayerIndex;
        int deLinkPlayerIndex;

        public List<int> linked; // a list to keep track of linekd characters
        public List<int> backup; // a backup list in case we have 2 separate linkings. 2 and 2.

        public bool completeLink;

        public List<Transform> playerList;
        public GameObject cameraRig;


        // Start is called before the first frame update
        void Start() {

            completeLink = true; // all players start linked
            playerList.Add(playerChar1);
            playerList.Add(playerChar2);
            playerList.Add(playerChar3);
            playerList.Add(playerChar4);
            currentPlayer = playerChar1;

        }

        // Update is called once per frame
        void Update() {

        }
        void OnGUI() {
            ChangeFollowers();
        }
        public void ChangeFollowers() {

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SwapToPlayer(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                SwapToPlayer(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                SwapToPlayer(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                SwapToPlayer(3);
            }

        }
        public void SwapToPlayer(int playerindex) {

            foreach (Transform player in playerList) {
                player.GetComponent<CharSwap>().currentPlayer = playerList[playerindex];
            }
            HandleFollowers();

            HandleControllers();


        }

        // Finds who the current player is, sets his controller, and turns off every other players controller
        void HandleControllers() {

            // if you are the current player, set controller, set distance to 0, and setup cameraRig
            currentPlayer.GetComponent<PlayerController>().enabled = true;
            currentPlayer.GetComponent<NavMeshAgent>().stoppingDistance = 0;
            cameraRig.GetComponent<CameraController>().currentPlayer = currentPlayer;

            foreach (Transform player in playerList) {

                // make sure every playercharacter knows who the current is
                player.GetComponent<Follower>().player = currentPlayer;

                if (player != currentPlayer) {
                    // disable all other player controllers and set stopping distance
                    player.GetComponent<PlayerController>().enabled = false;
                    player.GetComponent<NavMeshAgent>().stoppingDistance = 2;
                }

            }
        }

        // loop through each player, turn off everyones follower scripts, then check the linked and backup lists to determine who should follow who.
        // at the end, check if the lists are empty and completeLink is true. if it is, everyone follows the current player.
        void HandleFollowers() {
            // find the current player index
            for (int i = 0; i < playerList.Count; i++) {
                if (playerList[i] == currentPlayer) {
                    currentPlayerIndex = i;
                }
            }

            foreach (Transform player in playerList) {

                // before doing anything, turn off everyones follower script
                // we will turn it back on if 2 chars were linked
                player.GetComponent<Follower>().enabled = false;

                // if the currentplayer is linked to any other playerchar, we need to set the current player's followers' scripts on
                if (backup.Count > 0) { // backup is used, 2-2

                    // check what list the current player is in. linked or backup.

                    if (backup.Contains(currentPlayerIndex)) {
                        foreach (int existingIndex in backup) {
                            if (existingIndex != currentPlayerIndex) { // we found the follower, lets set the script
                                playerList[existingIndex].GetComponent<Follower>().enabled = true;
                            }
                        }
                    } else {
                        if (linked.Contains(currentPlayerIndex)) {
                            foreach (int existingIndex in linked) {
                                if (existingIndex != currentPlayerIndex) { // we found the follower, lets set the script
                                    playerList[existingIndex].GetComponent<Follower>().enabled = true;
                                }
                            }
                        }

                    }
                } else { // no backup used, so we either have a 12 or a 123
                    foreach (int existingIndex in linked) {
                        if (linked.Contains(currentPlayerIndex)) { // if the current player is in the list, get his followers, otherwise, leave him alone
                            if (existingIndex != currentPlayerIndex) { // we found a follower, lets set the script
                                playerList[existingIndex].GetComponent<Follower>().enabled = true;
                            }
                        }

                    }
                }
            }
            // last check, if completeLink is set to true, we set everyone who isnt the current Player to follow
            // we do this last because the loop before this sets all followers to false. and only enables the followers
            // if the list is populated. but when completeLink is true, the lists are always empty.
            if (completeLink) {
                foreach (Transform p in playerList) {
                    if (p != currentPlayer) {
                        p.GetComponent<Follower>().enabled = true;
                    }
                }
            }
        }

        // Links a player to another player by storing a reference to who is linked to who in a list.
        // there are 2 lists. linked and backup.
        // when a player is linked, that link is added to the 'linked' list until that list has 4 members in it. at that point, everyone is linked. empty the lists and set the boolean true
        // however, if you want to have 2 separately linked groups... i.e. player1-player2 | player3-player4
        // then that second team is used in the backup list.
        // determining whether you fill the linked list or the backup list is with the linked players respect to the already linked players
        public void LinkPlayer(int playerindex) {

            linkPlayerIndex = playerindex;
            // find the current player index
            for (int i = 0; i < playerList.Count; i++) {
                if (playerList[i] == currentPlayer) {
                    currentPlayerIndex = i;
                }
            }
            // pre-check to make sure BOTH indexes arent in the list (to prevent spamming the list)
            if (!PreCheck(currentPlayerIndex, linkPlayerIndex)) {
                return; // failed precheck, do nothing
            }

            // check if player is trying to link himself. if he is, he needs to join the already linked people
            // unless the lists are empty in which theres no one to link to so just return
            if (currentPlayerIndex == linkPlayerIndex) {
                if (!linked.Contains(currentPlayerIndex) && linked.Count > 0) {
                    foreach (Transform player in playerList) { // join the club
                        player.GetComponent<CharSwap>().linked.Add(currentPlayerIndex);
                    }
                    if (linked.Count == 4) { // we just made a completeLink, empty the lists and set value to true
                        foreach (Transform player in playerList) { // update every player with new linked info
                            player.GetComponent<CharSwap>().linked.Clear();
                            player.GetComponent<CharSwap>().backup.Clear();
                            player.GetComponent<CharSwap>().completeLink = true;
                        }
                    }
                    HandleFollowers();
                    return;
                } else {
                    return;
                }
            }

            // the next case to cover is the case where neither the current player nor the player we are handling are in the list at all BUT linked list is not empty
            // in this case, it means that you are creating a completely separate group. or 2 groups of 2.
            // we will place these 2 in the backup, handle followers, then return
            if (notInLinkedList(currentPlayerIndex, playerindex)) {
                if (linked.Count > 0) {
                    foreach (Transform player in playerList) { // update every player with new linked info
                        player.GetComponent<CharSwap>().backup.Add(currentPlayerIndex);
                        player.GetComponent<CharSwap>().backup.Add(linkPlayerIndex);
                    }
                    // assign new followers based on new information
                    HandleFollowers();
                    return;
                }

            }
            //  if both arent in the list, and both arent not in the list, that means that one of them is in the list but not the other, time to link.
            if (linked.Count > 0) {
                foreach (int linkedListIndex in linked) {
                    // if we find EITHER of the indexes in linked, we add the new link to the array.
                    // if we miss the mark, thats ok, move on.
                    if (linkedListIndex == currentPlayerIndex || linkedListIndex == linkPlayerIndex) {

                        if (backup.Count == 0) { // if we havent used the backup, this is a 3 person group ex. 12 3 4 = 123 4
                                                 // if an outsider asks to link to an insider, bring the insiders friends. 
                                                 // i.e  1-2 are linked.  3 link 2 should result in 3-1-2
                            if (linked.Contains(linkPlayerIndex)) {
                                foreach (Transform player in playerList) { // if linked contains the linkplayerindex, it means this is an outsider asking to come in.
                                    player.GetComponent<CharSwap>().linked.Add(currentPlayerIndex);
                                }
                            } else {
                                foreach (Transform player in playerList) { // otherwise its an insider asking an outsider to come in.
                                    player.GetComponent<CharSwap>().linked.Add(linkPlayerIndex);
                                }
                            }

                            if (linked.Count == 4) { // we just made a completeLink, empty the lists and set value to true
                                foreach (Transform player in playerList) { // update every player with new linked info
                                    player.GetComponent<CharSwap>().linked.Clear();
                                    player.GetComponent<CharSwap>().backup.Clear();
                                    player.GetComponent<CharSwap>().completeLink = true;
                                }
                            }
                            break;
                        } else { // the backup is used, so we are merging 2 groups of 2. ex. 12 34 = 1234
                            foreach (Transform player in playerList) {// update every player with new linked info
                                                                      // clean out both lists and mark completeLink true
                                player.GetComponent<CharSwap>().linked.Clear();
                                player.GetComponent<CharSwap>().backup.Clear();
                                player.GetComponent<CharSwap>().completeLink = true;
                            }
                            break;
                        }
                    }
                }
            } else { // this is the FIRST link between characters so add them both
                foreach (Transform player in playerList) { // update every player with new linked info
                    player.GetComponent<CharSwap>().linked.Add(currentPlayerIndex);
                    player.GetComponent<CharSwap>().linked.Add(linkPlayerIndex);
                }

            }

            // assign new followers based on new information
            HandleFollowers();
        }


        public void DelinkPlayer(int playerindex) {

            if (EmptyLists()) {
                if (completeLink == true) { // // if both lists are empty, and the link is complete, it means you need to populate the list with the rest of the players
                    for (int i = 0; i < 4; i++) { // fill the linked with the rest of the players
                        if (i != playerindex) {
                            foreach (Transform player in playerList) { // update every player with new linked info
                                player.GetComponent<CharSwap>().linked.Add(i);
                            }
                        }
                    }
                } else { // delinking when completeLink is false just means delinking an already delinked character. just dont do anything
                    return;
                }

            }
            foreach (Transform player in playerList) { // update every player with new linked info
                player.GetComponent<CharSwap>().completeLink = false; // if delink is ever called, we lost the complete link
            }
            deLinkPlayerIndex = playerindex;
            // if the backup is being used, we have 2-2. 
            if (backup.Count > 0) {
                if (backup.Contains(deLinkPlayerIndex)) {
                    foreach (Transform player in playerList) { // update every player with new linked info
                        player.GetComponent<CharSwap>().backup.Clear(); // know its a 2-2 situation so just get clear backup
                    }
                } else { // if it wasnt in the backup it must be in the 1st list. clean out the list and migrate the backup to the 1st list
                    foreach (Transform player in playerList) { // update every player with new linked info
                        player.GetComponent<CharSwap>().linked.Clear();
                        player.GetComponent<CharSwap>().linked.Add(backup[0]);
                        player.GetComponent<CharSwap>().linked.Add(backup[1]);
                        player.GetComponent<CharSwap>().backup.Clear();
                    }
                }
            } else { // no backup used, so we either have a 12 or a 123
                for (int i = 0; i < linked.Count; i++) {
                    if (linked[i] == deLinkPlayerIndex) {
                        foreach (Transform player in playerList) { // update every player with new linked info
                            player.GetComponent<CharSwap>().linked.RemoveAt(i);
                        }
                        if (linked.Count == 1) { // if removing the element caused only a single element in the list, just remove the links entirely
                            foreach (Transform player in playerList) { // update every player with new linked info
                                player.GetComponent<CharSwap>().linked.Clear();
                            }
                        }
                    }
                }
            }
            HandleFollowers();
        }


        // clean out both lists and mark completeLink true
        public void LinkAll() {
            foreach (Transform player in playerList) {// update every player with new linked info
                player.GetComponent<CharSwap>().linked.Clear();
                player.GetComponent<CharSwap>().backup.Clear();
                player.GetComponent<CharSwap>().completeLink = true;
            }

            // find the current player index
            for (int i = 0; i < playerList.Count; i++) {
                if (playerList[i] == currentPlayer) {
                    currentPlayerIndex = i;
                }
            }
            HandleFollowers();

        }

        // clean out both lists, set completeLink to False
        public void DelinkAll() {

            foreach (Transform player in playerList) {// update every player with new linked info
                player.GetComponent<CharSwap>().linked.Clear();
                player.GetComponent<CharSwap>().backup.Clear();
                player.GetComponent<CharSwap>().completeLink = false;
            }

            // find the current player index
            for (int i = 0; i < playerList.Count; i++) {
                if (playerList[i] == currentPlayer) {
                    currentPlayerIndex = i;
                }
            }
            HandleFollowers();

        }

        // pre-check to make sure BOTH indexes arent in the list (to prevent spamming the list)
        bool PreCheck(int cpIndex, int lIndex) {
            if (linked.Contains(cpIndex) && linked.Contains(lIndex)) {
                return false;
            }
            if (backup.Contains(cpIndex) && backup.Contains(lIndex)) {
                return false;
            }
            return true; // we didnt find both in either array, its good to go.
        }

        // checking if both lists are empty.
        bool EmptyLists() {
            if (linked.Count == 0 && backup.Count == 0) {
                return true;
            }
            return false;

        }

        // checks if the given player index and the current player are BOTH not in the linked list.
        bool notInLinkedList(int currentindex, int playerindex) {
            if (!linked.Contains(playerindex) && !linked.Contains(currentindex)) {
                return true;
            }
            return false;
        }

    }
}
