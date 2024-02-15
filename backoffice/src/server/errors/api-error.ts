export class ApiError extends Error {
  errors: { [key: string]: string[] };

  constructor(message: string, errors: { [key: string]: string[] }) {
    super(message);
    this.name = "ApiError";
    this.errors = errors;
  }
}
