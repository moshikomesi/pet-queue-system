export type User = {
  userId?: number;
  username?: string;
};

export type AuthContextType = {
  token: string | null;
  user: User | null;
  login: (jwt: string) => void;
  logout: () => void;
};
