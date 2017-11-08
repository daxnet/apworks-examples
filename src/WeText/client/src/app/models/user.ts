export class User {

    /**
     * Creates an instance of User.
     * @param {string} [userName] The name of the user to be created.
     * @param {string} [displayName] The nick name or the display name of the user.
     * @param {string} [email] The email address of the user.
     * @memberof User
     */
    constructor (public userName?: string,
        public displayName?: string,
        public email?: string,
        public avatarBackgroundColor?: string) {
    }
};
