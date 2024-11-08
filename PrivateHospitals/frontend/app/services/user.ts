export interface LoginRequest {
    Email: string,
    Password: string
};

export interface UserLogged {
    Email: string,
    Token: string
}
