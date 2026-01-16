'use client';

import { Container, Typography, Box, Button, Paper } from '@mui/material';
import { Dashboard as DashboardIcon } from '@mui/icons-material';

export default function Home() {
  return (
    <Container maxWidth="lg">
      <Box
        sx={{
          my: 4,
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
          alignItems: 'center',
          minHeight: '80vh',
        }}
      >
        <Paper
          elevation={3}
          sx={{
            p: 4,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            gap: 3,
            maxWidth: 600,
          }}
        >
          <DashboardIcon sx={{ fontSize: 60, color: 'primary.main' }} />
          <Typography variant="h3" component="h1" gutterBottom>
            LLM Dashboard
          </Typography>
          <Typography variant="body1" color="text.secondary" align="center">
            Welcome to your LLM Dashboard. This is a Next.js application with
            Material UI.
          </Typography>
          <Box sx={{ display: 'flex', gap: 2, mt: 2 }}>
            <Button variant="contained" color="primary">
              Get Started
            </Button>
            <Button variant="outlined" color="primary">
              Learn More
            </Button>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
}
