import { ApiError } from "@/server/errors/api-error";

interface GraphQLResponse<T> {
  data: T;
  errors?: GraphQLError[];
}

interface GraphQLError {
  message: string;
  locations: { line: number; column: number }[];
  path: string[];
  extensions: {
    code: string;
    errors: Record<string, string[]>;
  };
}

export async function fetchGraphQL<T>(
  query: string,
  variables: Record<string, any> = {}
): Promise<T> {
  const response = await fetch("http://localhost:5195/graphql", {
    headers: {
      "Content-Type": "application/json",
    },
    method: "POST",
    cache: "no-cache",
    body: JSON.stringify({ query, variables }),
  });

  const jsonResponse: GraphQLResponse<T> = await response.json();

  if (jsonResponse.errors) {
    const errorMessage = jsonResponse.errors[0]?.message;
    const errorDetails = jsonResponse.errors[0]?.extensions?.errors;

    if (errorDetails) {
      const errorObject: { [key: string]: string[] } = JSON.parse(
        errorDetails.toString()
      );

      throw new ApiError(errorMessage, errorObject);
    } else {
      console.error(jsonResponse.errors);
      throw new Error(errorMessage);
    }
  }

  return jsonResponse.data;
}
