import { PrismaClient } from "@prisma/client";

const prisma = new PrismaClient();

async function main() {
  const categories = [
    "Mercearia",
    "Especiarias",
    "Enlatados",
    "Limpeza",
    "Banho",
    "Roupa",
    "Lanche",
    "Sobremesas",
    "Gordices",
  ];

  for (const category of categories) {
    await prisma.category.create({
      data: {
        name: category,
      },
    });
  }

  const products = [
    {
      category: "Mercearia",
      products: [
        { name: "Arroz", quantity: 6, minimumStock: 5 },
        { name: "Massa", quantity: 9, minimumStock: 5 },
        { name: "Placas Lasanha", quantity: 2, minimumStock: 2 },
        { name: "Esparguete", quantity: 2, minimumStock: 2 },
        { name: "Pão Ralado", quantity: 1, minimumStock: 1 },
        { name: "Água", quantity: 2, minimumStock: 2 },
        { name: "Óleo", quantity: 0, minimumStock: 1 },
        { name: "Azeite", quantity: 0, minimumStock: 3 },
        { name: "Vinho", quantity: 2, minimumStock: 2 },
      ],
    },
    {
      category: "Especiarias",
      products: [
        { name: "Orégãos", quantity: 0, minimumStock: 1 },
        { name: "Pimentão Doce", quantity: 0, minimumStock: 1 },
        { name: "Sal Fino", quantity: 0, minimumStock: 1 },
        { name: "Sal", quantity: 1, minimumStock: 1 },
        { name: "Vaqueiro", quantity: 1.5, minimumStock: 1 },
        { name: "Pimenta Branca", quantity: 0, minimumStock: 1 },
        { name: "Vinagre", quantity: 1.5, minimumStock: 1 },
        { name: "Polpa De Tomate", quantity: 1, minimumStock: 1 },
        { name: "Piri-Piri", quantity: 0, minimumStock: 1 },
      ],
    },
    {
      category: "Enlatados",
      products: [
        { name: "Atum", quantity: 20, minimumStock: 5 },
        { name: "Salsicha", quantity: 0, minimumStock: 1 },
        { name: "Milho", quantity: 3, minimumStock: 1 },
        { name: "Cogumelos", quantity: 4, minimumStock: 4 },
        { name: "Grão", quantity: 2, minimumStock: 2 },
        { name: "Feijão Frade", quantity: 2, minimumStock: 1 },
        { name: "Feijão Preto", quantity: 3, minimumStock: 3 },
      ],
    },
    {
      category: "Limpeza",
      products: [
        { name: "Lixívia", quantity: 2, minimumStock: 2 },
        { name: "Esfregão", quantity: 2, minimumStock: 1 },
        { name: "Esponja", quantity: 1, minimumStock: 1 },
        { name: "Saco De Lixo", quantity: 3, minimumStock: 2 },
        { name: "Cheiro para Sanita", quantity: 5, minimumStock: 2 },
        { name: "Líquido Do Chão", quantity: 2, minimumStock: 1 },
        { name: "Líquido Da Loiça", quantity: 2, minimumStock: 2 },
        { name: "Rolo de Cozinha", quantity: 1, minimumStock: 2 },
      ],
    },
    {
      category: "Banho",
      products: [
        { name: "Shampoo", quantity: 3, minimumStock: 1 },
        { name: "Gel de Banho", quantity: 1, minimumStock: 1 },
        { name: "Toalhitas", quantity: 2, minimumStock: 1 },
        { name: "Escova de Dentes", quantity: 5, minimumStock: 2 },
        { name: "Pasta de Dentes", quantity: 2, minimumStock: 2 },
        { name: "Papel Higiénico", quantity: 35, minimumStock: 20 },
      ],
    },
    {
      category: "Roupa",
      products: [
        { name: "Amaciador", quantity: 2, minimumStock: 2 },
        { name: "Detergente", quantity: 1, minimumStock: 2 },
        { name: "Perfumador", quantity: 4, minimumStock: 2 },
      ],
    },
    {
      category: "Lanche",
      products: [
        { name: "Iogurtes", quantity: 12, minimumStock: 15 },
        { name: "Leite", quantity: 11, minimumStock: 4 },
        { name: "Tofina", quantity: 0, minimumStock: 1 },
        { name: "Manteiga", quantity: 3, minimumStock: 2 },
      ],
    },
    {
      category: "Sobremesas",
      products: [
        { name: "Farinha", quantity: 2, minimumStock: 2 },
        { name: "Açúcar", quantity: 1, minimumStock: 4 },
        { name: "Fermento", quantity: 1, minimumStock: 1 },
        { name: "Folha De Gelatina", quantity: 2, minimumStock: 2 },
        { name: "Leite Condensado", quantity: 6, minimumStock: 2 },
        { name: "Leite Condensado Cozido", quantity: 0, minimumStock: 2 },
        { name: "Bolacha Maria", quantity: 2, minimumStock: 4 },
        { name: "Chocolate em Pó", quantity: 1, minimumStock: 1 },
        { name: "Chocolate De Culinária", quantity: 1, minimumStock: 1 },
        { name: "Natas", quantity: 6, minimumStock: 10 },
      ],
    },
    {
      category: "Gordices",
      products: [
        { name: "Bacon", quantity: 0, minimumStock: 3 },
        { name: "Queijo Ralado", quantity: 4, minimumStock: 3 },
        { name: "Queijo", quantity: 0, minimumStock: 1 },
        { name: "Bechamel", quantity: 2, minimumStock: 3 },
        { name: "Ketchup", quantity: 0, minimumStock: 1 },
        { name: "Maionese", quantity: 0, minimumStock: 1 },
        { name: "Maionese De Alho", quantity: 1, minimumStock: 1 },
        { name: "Mostarda", quantity: 0, minimumStock: 1 },
      ],
    },
  ];

  products
    .map((pd) => pd.category)
    .forEach(async (category) => {
      const productsCategoryIndex = products.findIndex(
        (p) => p.category === category
      );

      for (const product of products[productsCategoryIndex].products) {
        await prisma.product.create({
          data: {
            name: product.name,
            quantity: product.quantity,
            minimumStock: product.minimumStock,
            categoryId: productsCategoryIndex + 1,
          },
        });
      }
    });
}

main()
  .catch(async (e) => {
    console.error(e);

    await prisma.$disconnect();

    process.exit(1);
  })
  .finally(async () => {
    await prisma.$disconnect();
  });
