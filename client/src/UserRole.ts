// mirrors the UserRole enum on the server
export enum UserRole {
    User = 1,
    SuperUser = 2,
    Admin = 3,
}

// the token stores the role by name ("Admin"), the API returns it as a number
export function roleFromName(name: string): UserRole | undefined {
    return UserRole[name as keyof typeof UserRole];
}

export function canManageUsers(role: UserRole | undefined): boolean {
    return role === UserRole.SuperUser || role === UserRole.Admin;
}
