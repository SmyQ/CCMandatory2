package com.example.imager;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FilenameFilter;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.InetSocketAddress;
import java.net.ServerSocket;
import java.net.Socket;

import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import android.os.Environment;
import android.os.Handler;
import android.util.Base64;
import android.view.Menu;
import android.widget.TextView;

public class MainActivity extends Activity {

	String host = "10.6.6.175";
	TextView message;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);       
        
        message = (TextView)findViewById(R.id.txtMessage);
        
//        Thread recThread = new Thread(receivePhotos);
//        recThread.start();
        
        Thread signalThread = new Thread(receiveTransmitSignal);
        signalThread.start();       

        
    }
    
    private final Handler mHandler = new Handler();
    
    public boolean isExternalStorageWritable() {
        String state = Environment.getExternalStorageState();
        if (Environment.MEDIA_MOUNTED.equals(state)) {
            return true;
        }
        return false;
    }

    
    public boolean isExternalStorageReadable() {
        String state = Environment.getExternalStorageState();
        if (Environment.MEDIA_MOUNTED.equals(state) ||
            Environment.MEDIA_MOUNTED_READ_ONLY.equals(state)) {
            return true;
        }
        return false;
    }
    
	public File[] finder(String dirName){
		Context c = getApplicationContext();
		File f = c.getFilesDir();

		FilenameFilter filter = new FilenameFilter() { 
	         public boolean accept(File dir, String filename)
             { return filename.endsWith(".jpg"); }};

    	return f.listFiles(filter);
    }
	
	
	Runnable receiveTransmitSignal = new Runnable(){
		@Override
		public void run(){
			ServerSocket serverSocket;
			int port = 8000;
			int hostport = 9000;
			
			try{
				serverSocket = new ServerSocket();
				serverSocket.bind(new InetSocketAddress("0.0.0.0", port));
				
				
				while(true){
					Post("Waiting... host: " + host );
					Socket socket = serverSocket.accept();
					
					BufferedReader fromClient = new BufferedReader(new InputStreamReader(socket.getInputStream()));
					DataOutputStream out = new DataOutputStream(socket.getOutputStream());

					String dataFromClient = fromClient.readLine();
					if(dataFromClient.equals("Transmit")){
						out.writeBytes("ACK\n");
						out.flush();
						Thread.sleep(100);
						
						Post("Transmit");
						
						try{
							FilenameFilter filter = new FilenameFilter() { 
						         public boolean accept(File dir, String filename)
					             { return filename.endsWith(".jpg"); }};
					             
							File dir = new File(Environment.getExternalStorageDirectory().getPath());
							File[] files = dir.listFiles(filter);
							
							out.writeBytes(String.valueOf(files.length) + "\n");
							out.flush();
							Thread.sleep(200);			
							for(File file : files){
								
								
								Post(file.getName());
												
								Socket outSocket = new Socket(host, hostport); //REMEMBER TO UPDATE HOST
								out = new DataOutputStream(outSocket.getOutputStream());
								
								Long fileSize = file.length();

								
								out.writeBytes(Long.toString(fileSize) + "\n");
								out.flush();
								Thread.sleep(100);
								out.writeBytes(file.getName() + "\n");
								out.flush();
								Thread.sleep(100);
													
								System.out.println("Sent filesize: " + fileSize);					
								
								byte[] buffer = new byte[fileSize.intValue()];
								FileInputStream fs = new FileInputStream(file);
								BufferedInputStream bs = new BufferedInputStream(fs);
								int read = bs.read(buffer, 0, buffer.length);
								Thread.sleep(100);
															
								OutputStream os = outSocket.getOutputStream();
								os.write(buffer, 0, read);			
									
								System.out.println("Closing.... Sent: " + read);
								
								os.flush();
								os.close();
								bs.close();
								fs.close();
								outSocket.close();
								
								Post("Done");
								
							}
							
						}
						catch(Exception e){
							Post(e.getMessage());
							socket.close();
						}					

					}
					else if(dataFromClient.equals("Receive"))
					{
						Post("Receive");
						
						try{
						File dir = new File(Environment.getExternalStorageDirectory().getPath());
																	
						
							InputStream isRec = socket.getInputStream();
							
							BufferedReader fromClientRec = new BufferedReader(new InputStreamReader(socket.getInputStream()));
							String dataFromClientRec = fromClientRec.readLine();
							int fileSize = Integer.parseInt(dataFromClientRec);
							String fileName = fromClientRec.readLine();
							Post("Filename: " + fileName);
										
							byte[] buffer = new byte[fileSize];
							FileOutputStream output = new FileOutputStream(dir.getAbsolutePath() + "/" + fileName, false);
							BufferedOutputStream bOut = new BufferedOutputStream(output);							
									
							int bytesRead = isRec.read(buffer, 0, fileSize);
							int current = bytesRead;
							do{
								bytesRead = isRec.read(buffer, current, fileSize-current);
								if(bytesRead >= 0) current += bytesRead;
								System.out.println("Current: " + current + " fileSize: " + fileSize + " bytes read: " + bytesRead);				
							}
							while(bytesRead > 0);
							
							bOut.write(buffer, 0, current);
							bOut.flush();
							output.flush();
//							socket.close();	
							
							Post("Received: " + fileName);						
						
											
						
						}
						catch(Exception e){
							Object o = e.getCause();
							Post(e.getMessage());
							socket.close();
						}
					
					}
			
			}}
			catch(Exception e){
				Post(e.getMessage());
			
			}
		}
	};
    
    
    Runnable receivePhotos = new Runnable(){

		@Override
		public void run() {
			
			ServerSocket serverSocket;
			
			try{
			serverSocket = new ServerSocket(8000);
			File dir = new File(Environment.getExternalStorageDirectory().getPath());
			while(true){
								
				
				System.out.println("Waiting for connection...");
				Socket socket = serverSocket.accept();
				System.out.println("Connection accepted...");			
				
				InputStream is = socket.getInputStream();
				
				BufferedReader fromClient = new BufferedReader(new InputStreamReader(socket.getInputStream()));
				String dataFromClient = fromClient.readLine();
				int fileSize = Integer.parseInt(dataFromClient);
				String fileName = fromClient.readLine();
				Post("Filename: " + fileName);
							
				byte[] buffer = new byte[fileSize];
				String abPath = dir.getAbsolutePath();
				FileOutputStream output = new FileOutputStream(dir.getAbsolutePath() + "/" + fileName, false);
				BufferedOutputStream bOut = new BufferedOutputStream(output);	
				
						
				int bytesRead = is.read(buffer, 0, fileSize);
				int current = bytesRead;
				do{
					bytesRead = is.read(buffer, current, fileSize-current);
					if(bytesRead >= 0) current += bytesRead;
					System.out.println("Current: " + current + " fileSize: " + fileSize + " bytes read: " + bytesRead);				
				}
				while(bytesRead > 0);
				
				bOut.write(buffer, 0, current);
				bOut.flush();
				bOut.close();
				output.close();
				socket.close();	
				
				Post("Received: " + fileName);
				
				
			}
			
			
			}
			catch(Exception e){
				System.out.println(e.getMessage());
				
			}

			
		}
    	
    };


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }
    
    public void Post(final String msg){
  		mHandler.post(new Runnable(){

			@Override
			public void run() {
				message.setText(msg);
				
			}
  			
  		});
    }
    
}




//	PostMessage("Saving string to disk");
//	Context context = getApplicationContext();
//	File dir = context.getFilesDir();
//	PostMessage(dir.getAbsolutePath());
//	
//	FileOutputStream fs = openFileOutput("test.txt", Context.MODE_WORLD_READABLE);
//	fs.write("Test string".getBytes());
//	fs.close();
//	
//	File[] files = dir.listFiles();
//	
//	File f = new File("test.txt");
//	
//	
//	
//	FileInputStream is = openFileInput("test.txt");
//	DataInputStream in = new DataInputStream(is);
//	
//	String str = null;
//	StringBuffer storedString = new StringBuffer();
//	
//	if(((str = in.readLine())) != null)
//		storedString.append(str);
//	
//	PostMessage("From file: " + storedString);
	
	
	
	
//	PostMessage("Waiting for connection...");		      		
//
//	Socket connectionSocket = socket.accept();
//	
//	PostMessage("Connection accepted");
//	
//	BufferedReader fromClient = new BufferedReader(new InputStreamReader(connectionSocket.getInputStream()));
//	String dataFromClient = fromClient.readLine();
//	
//	dataOutputStream = new DataOutputStream(connectionSocket.getOutputStream());
//	PostMessage("Received: " + dataFromClient);
//	if(dataFromClient.equals("ReqUserName"))
//		dataOutputStream.writeBytes("rylander86\n");
//


//ServerSocket socket = null;
//DataOutputStream dataOutputStream = null;
//DataInputStream dataInputStream = null;
//try{
//socket = new ServerSocket(8001);
//
//
//	while(true){
//		
//
//		Thread.sleep(5000);
//	}
//}
//catch (IOException e){
//	  TextView t = (TextView)findViewById(R.id.txtMessage);
//	  t.setText(e.getMessage());
//	
//} catch (InterruptedException e) {
//	// TODO Auto-generated catch block
//	e.printStackTrace();
//}