# LLM Dashboard Frontend

A Next.js + React + Material UI application for managing and monitoring LLM prompts with real-time status updates.

## Tech Stack

- Next.js 16 (App Router)
- React 19
- Material UI v7
- TypeScript

## Prerequisites

- Node.js 20+ (LTS recommended)
- npm

## Quick Start

```bash
# Install dependencies
npm install

# Set API URL (optional, defaults to http://localhost:8080)
echo "NEXT_PUBLIC_API_URL=http://localhost:8080" > .env.local

# Run development server
npm run dev
```

Open [http://localhost:3000](http://localhost:3000)

## Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm start` - Start production server
- `npm run lint` - Run ESLint

## API Endpoints

Base URL: `http://localhost:8080` (configurable via `NEXT_PUBLIC_API_URL`)

- `GET /api/prompt` - Get all prompts
- `POST /api/prompt` - Create prompt `{ "text": "..." }`
- `DELETE /api/prompt/{id}` - Delete prompt

## Project Structure

```
app/
├── components/     # UI components
├── lib/
│   ├── services/   # API client
│   └── types/      # TypeScript types
├── theme/          # Material UI theme
└── page.tsx        # Main dashboard
```

## Deployment

Deploy to Vercel, Netlify, or any Next.js-compatible platform. Set `NEXT_PUBLIC_API_URL` environment variable.

---

Built with Next.js and Material UI
