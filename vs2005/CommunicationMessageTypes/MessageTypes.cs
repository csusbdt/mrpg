using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationMessageTypes
{
    public class MessageTypes
    {
        // Client messages

        public const string LOGIN = "login"; 
        public const string LOGOUT = "logout"; 
        public const string SELECT_AVATAR = "sel_avatar"; 
        public const string EXIT_GAME = "exit"; 
        public const string MOVE_REQUEST = "move_request"; 
        public const string ACTION_REQUEST = "action_request"; 
        public const string STOP_ACTION_REQUEST = "stop_action_request"; 
        public const string INTERACT_REQUEST = "interact_request";
        public const string ACQUIRE_ITEM_REQUEST = "acquire_item_request";

        // Server messages

        public const string LOGIN_SUCCESS = "login_success";
        public const string LOGIN_FAILURE = "login_failure";

        public const string AVATAR_LIST = "avatars";

        public const string SET_MAP = "set_map";
        public const string CREATE_MODELED_ENTITY = "create_modeled_entity";
        public const string SET_PC = "set_pc";
        public const string ADD_CAPABILITY = "add_capability";
        public const string REMOVE_CAPABILITY = "remove_capability";
        public const string ADD_ITEM_TO_INVENTORY = "add_inv_item";
        public const string REMOVE_ITEM_FROM_INVENTORY = "remove_inv_item";

        public const string CREATE_ITEM_LIST = "create_item_list";
        public const string ADD_ITEM_TO_LIST = "list_item";
        public const string REMOVE_ITEM_FROM_LIST = "delist_item";

        public const string CREATE_ACTION = "action";
        public const string STOP_ACTION = "stop_action";
        public const string SET_MANA = "mana";
        public const string SET_HEALTH = "health";

        public const string MOVE_ENTITY = "move";
        public const string DIE = "die";
        public const string DELETE_ENTITY = "delete";
    }
}
